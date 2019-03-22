// 
// TempCer.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

using System;
using System.Security.Cryptography.X509Certificates;
using Ultz.Extensions.PrivacyEnhancedMail;

namespace DemoOne
{
    public static class TempCer
    {
        public static X509Certificate2 Get()
        {
            return new X509Certificate2
            (
                Convert.FromBase64String
                (
                    "MIIKegIBAzCCCjYGCSqGSIb3DQEHAaCCCicEggojMIIKHzCCB" +
                    "ZAGCSqGSIb3DQEHAaCCBYEEggV9MIIFeTCCBXUGCyqGSIb3DQ" +
                    "EMCgECoIIE7jCCBOowHAYKKoZIhvcNAQwBAzAOBAhkchMWPox" +
                    "XPwICB9AEggTIKQKcIE/TxAmKw4pjyApPy/2fnnhmXlcV52d7" +
                    "VPoCT77Fz8bde7rDMEMsVFhDOgwmDQtPN7VHF65mPR34TcqlX" +
                    "gETlet1Fxh2Bf6xK2qqSLM9/DGB40c0piJeiwZc/24b3021wd" +
                    "GQwQH4fqAUWAGQsxuaYjJ4UzAXWLoEA548tsqKN5KiwoD24j9" +
                    "C1R8MKToMAEO3Kz9UwkadxbHpsWOCPsB4h0k2C3utPT+AUpTJ" +
                    "RmDSlt95M+HQe9R+3OWnVei95tk1h/q0Me59Hie3gMzAW7JAr" +
                    "OVjKL/lJofhcelMFngFfoav4twuffO3pm9D4BOPQMyn4tKfAG" +
                    "vxe3wpJqduDE2OqYK4xuv7eSf5yeofEuuxFimryQteXLMzR3H" +
                    "PwTxUDzsTEctB6HtTAAxOZbFdiyjOyADKxq3JktUcU4S+1SEp" +
                    "n7DjxYeS6mU+Xmf79Mz9PrdCsosKkm3TWeL+NplpJeG0h0qUW" +
                    "Q+cG4iB/i0gHKBP9RnDc17tLf5BFz092lp+EtVbNZeHPKZcLd" +
                    "jNfPTMuTVNnmxis+E0r+XV2SJTOGdLgzreiU0i4onAAXl1Z4C" +
                    "36AvYquKiPeXCPM8bBbA+8JT+BsEdnnkxbck6Mwby//EfYdc6" +
                    "sOeczLB1xdefriH3xrZjkE/DtsQzND5R1KRDRSirQzELs91st" +
                    "GYFoufwji6bOmEKIxF2wLsYao3EGGoKKy0jKZ6H1vZJ5g8mfO" +
                    "aDmeXx4ft6rhseZMPZA1twMWZNz4XIT+KWN8OXeGSMEXCYAZl" +
                    "JLhl+xfQffMqesui648PRiyn/CkfrnoEs1U7ucZuBKZZxKSRo" +
                    "k+2/1L9R5yWbcim/mLfSy7718ZraKgL0+JO1TdPH7dQBVLYIx" +
                    "gg8JuSewgC24SROximrrfpXnbCyiR3oGXJmrHYz063WphpZCc" +
                    "M8a4kgsVzO91PcWYFBq77blkG5Qgw9PHt+3rYyqpEzHEuUmjh" +
                    "hqHX6dzm96lZBmV6eI/165WpKiYuiYNQ20KgYahww5QhyV0Qq" +
                    "QmkBUkAlipbo+LG6BSc3Ud3gxYCGij6VawxUOklILISIZGmYr" +
                    "Ed/jfIle2hj5QMn5aw4mG25AvjdS56XS42eWSazkoMgQCUxGd" +
                    "SBOaRGhthMHZ+LyCvvcQnnI0bYNY04ChQfz9IkgS3V87ilHYN" +
                    "UWnK0oJXJrpLpzeQe2he4qS69oJ6vuxkimxlqWeQzMaGEUc8G" +
                    "YsLqSNxkI3N540wBEIAsixEgDCa5ktsYmKsFb4qUAdBnpvIGw" +
                    "P64HMEebMbdgtbIK+ZiGEQVx5FlNSxSxmRDZ9c1FQIeGch3OX" +
                    "v4gf+spv3/t5JQWT4hKRQG8J5w2j3QpexQKd5kXNvTJDC+E1Z" +
                    "qlG18Ymqvtz4UbwDjjftBK/YrFntJ2y5TztnXxtJIlpp6hoCs" +
                    "5dBMR7r3dvOqYzZyf+EWVXeZne+3pKRLXJZGxfMhgL1x7uVEk" +
                    "yyMyvTneoNH35OeSqaoCOeGR03sLD3xPW1/v6natt8pJLfGXA" +
                    "07HOX8g229urxEVMifnJX5oCwV5is5qN+4/UXXnZeQKGOQjIe" +
                    "BrtekwIKfvPXfNBryqeejG4qpwA+xK+GuWL66siie1gGG/mHE" +
                    "7mwISjF6yQwO8GVAspsjyUL8xfLQMXQwEwYJKoZIhvcNAQkVM" +
                    "QYEBAEAAAAwXQYJKwYBBAGCNxEBMVAeTgBNAGkAYwByAG8Acw" +
                    "BvAGYAdAAgAFMAbwBmAHQAdwBhAHIAZQAgAEsAZQB5ACAAUwB" +
                    "0AG8AcgBhAGcAZQAgAFAAcgBvAHYAaQBkAGUAcjCCBIcGCSqG" +
                    "SIb3DQEHBqCCBHgwggR0AgEAMIIEbQYJKoZIhvcNAQcBMBwGC" +
                    "iqGSIb3DQEMAQMwDgQINai/vlDLijgCAgfQgIIEQJaKOnms/h" +
                    "tXkXHiiK3ATOmsjD/s7s2QfNaPTSI0fML/W0HR32FP65w9cjr" +
                    "gGQ4urX2qKOvD2l77Iod3OWI0BCcFQYy54D1R17i953J6iD8x" +
                    "So3OeALj741xZm2RcsxT3WT3ZWs4lCkN44JOzVFxa1YowJwEQ" +
                    "6+bS4Y2g1gFXq0slQnvP/+bLrbpgwrpUm4YnNTvY3eCnbxi5O" +
                    "HzaU9LjPyvMmI0OqTqSSidRIx70FXi4QFzEz/Ys7Z7UXQOksm" +
                    "PCf32oNl/KajQGIX/HJKay2nI6afJjwYXFiGtuUYWgTYJ1PzR" +
                    "6Pt7dUxWWgJzOZkmJSG6H3nyGBZNttqjwp2dXN1kpi8qR4KS0" +
                    "Rs6VXLg5+qG+xbGO6xEtj1Ykc2pUuhgUbFUsApVAPwv+zWWuT" +
                    "ioWZlV5aUh50X3YVrDNC+AV3NCbw757QaEAxroB2eVVCb0jhw" +
                    "xRmQPdgaIlt8oS7TlP4rFqY9G+cUAH1xeHQo8D0AEFaNr/dOS" +
                    "/fWG0A+xXMGYyw57oxlwgLc5kJyiMfjDuoGFofXvVoHSINx2X" +
                    "UvasyOAWxtC3u4jXTjdHzhmWKAIL6dSNlP6haLWUU5qyYEqAF" +
                    "U/xxaLSwwxMSrSWYSD7YShUgmi4DqqUj4nROTGNmBvL8ld1aD" +
                    "3V55VZRSzkTarj1V+jP415mSLt7aZvm/7aNfkoj86BNXAY3+/" +
                    "Vdl3loxLd//oDu5749PRtpL/magekH/yeLCZQdmBOUrnWgY8r" +
                    "ux7SOTH/HpCP609FGd77TmCt8uGVL+V8ejicwORr0OW1DEZmt" +
                    "9KWhDb/hbb4ZzE8or4Wno1LJVd5RF9eC6baCdqFS9nOLtqrPq" +
                    "cHdMhwKmv4IErxInTJrB65/yEMf9IUi2RoRmsX13BIxil5mlO" +
                    "hAFYK+2SleaSpz61gv/4WnNPG7x42Sq43DbLDVHEBb4q+W3b1" +
                    "s8IOHpBHYPUTwzoFNPAxdh1Wz1n46BHAuW9kxOF/PNI61O4ZH" +
                    "ji6G+oj0oX9ApRT8JAbbA8Cy9RJ5s8Q0FEpCvYzz9fANWqUj7" +
                    "gnpPkg5PLbt9dJ066MfhEZmFG0IIiWfKtDfGmGzcC6D9CMsaP" +
                    "CHXGSw11qrUnRp/s3b8qJxgzRy87M3RhthBnHYDOPV0Erf20P" +
                    "N6GjPUI6qqPEAkfgoOKXXYeZD6viB50H6eshhcUrLBYzNFQOF" +
                    "U7H8Bo7JAhqT3NCrL6G0ufiyj2HQkdj3jK7qwE8/4ayvgHjAN" +
                    "15fNPPyM3Y/IvLKxaN1ef97mjwf9yVqJY186VB7rHTi9E8UDE" +
                    "jLSDSSsboW1JrN4RYjzYrD+d2DzT1wRYDI2GnH6PEM1abj0qB" +
                    "6Bwj7GQAhqPXHg1ywKFMUjGVmLdKz8dfFYlNWhVf9FIPPySbD" +
                    "YCCIJ+NE0LnSRaa7OhxKDksvjpC+oGXahF2GqGZPEPqAeV+hO" +
                    "jPl929AMn6BZY8DtZ8U/jMDswHzAHBgUrDgMCGgQUItp/+xy0" +
                    "vZSrmpTQdLsE6kDnltAEFBWmh+W4Dzn3a6LoOtNGfbxyaB/CA" +
                    "gIH0A=="
                )
            );
        }
    }
}
