﻿#region License
/*
 * Decoder.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion
//#define SOCKET_IO_DEBUG
//#define SOCKET_IO_DEBUG			// Uncomment this for debug
using SimpleJSON;
using System;
using System.Text;
using UnityEngine;
using WebSocketSharp;
namespace SocketIO
{

	public class Decoder
	{
		public Packet Decode(MessageEventArgs e)
		{
			try
			{
				#if SOCKET_IO_DEBUG
				Debug.Log("[SocketIO] Decoding: " + e.Data);
				#endif

				string data = e.Data;

				Packet packet = new Packet();
				int offset = 0;

				// look up packet type
				int enginePacketType = int.Parse(data.Substring(offset, 1));
				packet.enginePacketType = (EnginePacketType)enginePacketType;

				if (enginePacketType == (int)EnginePacketType.MESSAGE) {
					int socketPacketType = int.Parse(data.Substring(++offset, 1));
					packet.socketPacketType = (SocketPacketType)socketPacketType;
				}

				// connect message properly parsed
				if (data.Length <= 2) {
					#if SOCKET_IO_DEBUG
					Debug.Log("[SocketIO] Decoded: " + packet);
					#endif
					return packet;
				}

				// look up namespace (if any)
				if ('/' == data [offset + 1]) {
					StringBuilder builder = new StringBuilder();
					while (offset < data.Length - 1 && data[++offset] != ',') {
						builder.Append(data [offset]);
					}
					packet.nsp = builder.ToString();
				//	Debug.Log("packet.nsp: "+ packet.nsp);
				} 
                else {
					packet.nsp = "/";
				}


				// look up id
				char next = data [offset + 1];
				if (next != ' ' && char.IsNumber(next)) {
					StringBuilder builder = new StringBuilder();
					while (offset < data.Length - 1) {
						char c = data [++offset];
						if (char.IsNumber(c)) {
							builder.Append(c);
						} else {
							--offset;
							break;
						}
					}
					packet.id = int.Parse(builder.ToString());
					
						
				}

				// look up json data
				if (++offset < data.Length - 1) {
					try {
						#if SOCKET_IO_DEBUG
						Debug.Log("[SocketIO] Parsing JSON: " + data.Substring(offset));
#endif

                        if (SocketIOComponent.dicPacket.ContainsKey(packet.id))
                        {
                          //  debug.Log("<color=lime>packet.id: " + packet.id + " Parse data JSONNode </color>");
                            JSONNode json = JSONNode.Parse(data.Substring(offset));
							packet.jsonnodeParse = json;
                            SocketIOComponent.dicPacket.Remove(packet.id);
                        }
                        else if (SocketIOComponent.dicPacketLua.ContainsKey(packet.id))
                        {
							string str = data.Substring(offset); ;
                            if (str.StartsWith("[") && str.EndsWith("]"))
                            {
                                str = str.Substring(1, str.Length - 2);  // Cắt bỏ dấu đầu và cuối
                            }
                            str = str.Replace("\\", "");
                            if (str.StartsWith("[") && str.EndsWith("]"))
                            {
                                str = str.Substring(1, str.Length - 2);  // Cắt bỏ dấu đầu và cuối
                            }
                            if (str.StartsWith("\"") && str.EndsWith("\""))
                            {
                                // Loại bỏ dấu nháy kép ở đầu và cuối, thay thế bằng dấu nháy đơn
                                str = str.Substring(1, str.Length - 2);  // Cắt bỏ dấu đầu và cuối
                            }

                            packet.str = str;
                            // debug.Log("<color=lime>packet.id: " + packet.id + " Parse dataSTR </color> " + packet.str);
                            SocketIOComponent.dicPacketLua.Remove(packet.id);

                        }
                        else
						{
                            packet.json = new JSONObject(data.Substring(offset));
                        }

                        
                        //Debug.Log("json parse packet = " + json.ToString());
                    } catch (Exception ex) {
						Debug.LogException(ex);
					}
				}

				#if SOCKET_IO_DEBUG
				Debug.Log("[SocketIO] Decoded: " + packet);
				#endif

				return packet;

			} catch(Exception ex) {
				throw new SocketIOException("Packet decoding failed: " + e.Data ,ex);
			}
		}
    }
}
