
using SimpleJSON;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;
using Product = UnityEngine.Purchasing.Product;

public class inappload : MonoBehaviour, IStoreListener
{
    CrGame crgame; public GameObject RestoreButton; NetworkManager net;
    public string[] skimcuong = new string[] { "com.memobi.studio.300kimcuong", "com.memobi.studio.1500kimcuong", "com.memobi.studio.3500kimcuong" };
    public string[] sokimcuong = new string[] { "300", "1500", "3500" };
    public string[] sotien = new string[] { "19.000Đ", "99.000Đ", "199.000Đ"};
     IStoreController m_StoreController;
    void Awake()
    {
        crgame = GetComponent<CrGame>();
        //if(Application.platform != RuntimePlatform.IPhonePlayer)
        //{
        //    RestoreButton.SetActive(false);
        //}    
        net = GetComponent<NetworkManager>();
        InitializePurchasing();
    }
    //void Start()
    //{
    //    Debug.Log("Startttttttttttttttttttttttt");
    //}    
    public void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        //Add products that will be purchasable and indicate its type.
        for (int i = 0; i < skimcuong.Length; i++)
        {
            builder.AddProduct(skimcuong[i], ProductType.Consumable);
        }
        UnityPurchasing.Initialize(this, builder);
    }
    public void MuaKimCuong()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        int index = btnchon.transform.GetSiblingIndex();
        crgame.OnThongBaoNhanh("Đang khởi tạo giao dịch...", 2);
        crgame.panelLoadDao.SetActive(true);
        string id = skimcuong[index];
        debug.Log("id: " + id);
        m_StoreController.InitiatePurchase(id);
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Mua hàng trong ứng dụng đã khởi tạo thành công!");
        
        m_StoreController = controller;
    }
    public void OnInitializeFailed(InitializationFailureReason error, string? message = null)
    {
        crgame.OnThongBaoNhanh("Không thể khởi tạo giao dịch mua");
        var errorMessage = $"Không thể khởi tạo giao dịch mua. Lý do: {error}.";

        if (message != null)
        {
            errorMessage += $" Thêm chi tiết: {message}";
        }

        Debug.Log(errorMessage);
    }
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Truy xuất sản phẩm đã mua
        crgame.panelLoadDao.SetActive(false);
        var product = args.purchasedProduct;
        XacThuc(args);
        return PurchaseProcessingResult.Complete; // Chờ xác thực từ server

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        //string receipt = args.purchasedProduct.receipt; // Lấy hóa đơn
        // StartCoroutine(VerifyPurchase(receipt));
 
      //  return PurchaseProcessingResult.Complete;
    }
    private void XacThuc(PurchaseEventArgs args)
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "NapInApp";
        datasend["method"] = "XacThuc";
        datasend["data"]["receipt"] = args.purchasedProduct.receipt;
        datasend["data"]["platform"] = Application.platform == RuntimePlatform.Android ? "google" : "apple";
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                // Debug.Log("Xác thực thành công: " + www.downloadHandler.text);
                m_StoreController.ConfirmPendingPurchase(args.purchasedProduct);

                //string kc = "300";
                ////product.definition.payout.
                //if (args.purchasedProduct.definition.id == skimcuong[0])
                //{
                //    Debug.Log("Mua 300kc thành công");
                //}
                //else if (args.purchasedProduct.definition.id == skimcuong[1])
                //{
                //    Debug.Log("Mua 1500kc thành công"); kc = "1500";
                //}
                //else if (args.purchasedProduct.definition.id == skimcuong[2])
                //{
                //    Debug.Log("Mua 3500kc thành công"); kc = "3500";
                //}
                CrGame.ins.OnThongBao(true,json["message"].AsString,true);

                //net.socket.Emit("NapInApp", JSONObject.CreateStringObject(kc));

                Debug.Log($"Purchase Complete - Product: {args.purchasedProduct.definition.id}");
               // PurchaseProcessingResult.Complete;

            }
            else
            {
                CrGame.ins.OnThongBao(true, json["message"].AsString, true);
            }
        }

    }    
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        crgame.panelLoadDao.SetActive(false);
        crgame.OnThongBaoNhanh("Giao dịch thất bại");
        Debug.Log($"Giao dịch thất bại - Product: '{product.definition.id}', Mua hàngThất bại Lý do: { failureReason}");
    }
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        crgame.OnThongBaoNhanh("Lỗi khi khởi tạo");
        Debug.Log("Lỗi OnInitializeFailed " + error);
        //throw new NotImplementedException();
    }
    public void MuaKimCuongThatBai(Product product, PurchaseFailureReason failureReason)
    {
        crgame.panelLoadDao.SetActive(false);
        crgame.OnThongBaoNhanh("Giao dịch thất bại..");
        Debug.Log(product.definition.id + " nạp thất bại vì " + failureReason);
    }
}