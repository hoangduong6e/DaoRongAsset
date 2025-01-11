using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HacLongAttack : DragonPVEController
{
    private float timeCDbiDong;
    public bool BiDong = false;
    private void StartBiDong()
    {
        if(BiDong)
        {
            timeCDbiDong = timeCDskill;

            HapHuyetBiDong();
        }
    }

    private void AutoKichHoatSkill()
    {
        string nameskill = "";
        float[] probabilities = { 0.4f, 0.3f, 0.3f }; // Xác suất: 40%, 40%, 20%
        float randomPoint = Random.value; // Random từ 0.0 đến 1.0

        if (randomPoint < probabilities[0]) // 40%
        {
            nameskill = "CuongNo";
        }
        else if (randomPoint < probabilities[0] + probabilities[1]) // 30%
        {
            nameskill = "HapHuyet";
        }
        else // 30%
        {
            nameskill = "DoatMenh";
        }
        FuncInvokeOnline(nameskill,false);
    }
    private bool isCuongNoStart = false;
    private void KichHoatCuongNoStart()
    {
        if(!isCuongNoStart)
        {
            isCuongNoStart = true;
            CuongNo();
        }
    }
    private void UpdateBiDong()
    {
        //if (BiDong)
        //{
        //    if (isCuongNoStart) // nếu như đã dùng skill cuồng nộ khi gặp địch lần đầu thì mới tính random skill tiếp theo
        //    {
        //        if (timeCDbiDong > 0)
        //        {
        //            timeCDbiDong -= Time.deltaTime;
        //        }
        //        else
        //        {
        //            AutoKichHoatSkill();
        //            timeCDbiDong = timeCDskill;
        //        }
        //    }
        //}
    }

    private void HapHuyetBiDong(float nhan = 1)
    {
        _HutHp = 60;
        VienChinh.vienchinh.SetBuffHutHpall(HutHpChoToanDoi * nhan, team);
        if (nhan == 1)
        {
            if (team == Team.TeamXanh)
            {
                VienChinh.vienchinh.HienIconSkill(999999, "Xanh", "iconHapHuyetXanh");
            }
            else
            {
                //  ImgHp.sprite = VienChinh.vienchinh.thanhmaudo;
                VienChinh.vienchinh.HienIconSkill(999999, "Do", "iconHapHuyetDo");
            }
            VienChinh.vienchinh.SetBuffHutHpall(10,team);
            isHapHuyet = true;
        }
        else
        {
            if (team == Team.TeamXanh)
            {
                VienChinh.vienchinh.TaticonSkill("iconHapHuyetXanh","Xanh");
            }
            else
            {
                //  ImgHp.sprite = VienChinh.vienchinh.thanhmaudo;
                VienChinh.vienchinh.TaticonSkill("iconHapHuyetDo", "Do");
            }
            VienChinh.vienchinh.SetBuffHutHpall(-10, team);
            isHapHuyet = false;
        }
    }
}
