using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.Utils.Helpers
{
    public static class RandomHelper
    {
        public static string CreateCode(int codeCount)
        {
            const string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            int t = -1;
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }

                while (temp == (t = rand.Next(35))) { }

                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }

        public static string CreateChease(int intLength)
        {
            string strChinese = string.Empty;
            Random random = new Random(DateTime.Now.Millisecond);
            Encoding gbEncoding = Encoding.GetEncoding("gb2312");
            for(int i = 0; i < intLength; i++)
            {
                //获取区码（常用汉字的区码范围为16-55）
                int regionCode = random.Next(16, 56);
                
                //获取位码（位码范围为1-94 由于55区的90，91，92，93，94为空，帮将其排除）
                int positionCode = random.Next(1, regionCode == 55 ? 90 : 95);

                //转换欧码为机内码
                int regionCode_Machine = regionCode + 160; //160即为十六进制的20H+80H=A0H
                int positoinCode_Machine = positionCode + 160;//160即为十六进制的20H+80H=A0H

                //转换为汉字
                byte[] bytes = new byte[] { (byte)regionCode_Machine, (byte)positoinCode_Machine };
                strChinese += gbEncoding.GetString(bytes);
            }
            return strChinese;
        }
    }
}
