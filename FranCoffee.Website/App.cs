using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TNT.Core.Helpers.Smtp;

namespace FranCoffee.Website
{
    public class App
    {
        public string Name { get; set; }

        private string[] _supportedCultures;
        public string[] SupportedCultures
        {
            get
            {
                return _supportedCultures;
            }
            set
            {
                _supportedCultures = value;
                if (value != null)
                    SupportedCulturesInfo = value.Select(c => new CultureInfo(c)).ToArray();
            }
        }

        public CultureInfo[] SupportedCulturesInfo { get; set; }

        public string FromMailInfo { get; set; }
        public string ToMailAddress { get; set; }

        public SmtpGmailManager GmailManager { get; set; }

        public FacebookChatPlugin FbChat { get; set; }

        private static App instance;
        public static App Instance
        {
            get
            {
                if (instance == null)
                    instance = new App();
                return instance;
            }
        }

    }

    public class FacebookChatPlugin
    {
        public string PageId { get; set; }
        public string ThemeColor { get; set; }
        public string LoginGreeting { get; set; }
        public string LogoutGreeting { get; set; }
    }

    public static class RandomExtensions
    {
        public static void Shuffle<T>(this Random rng, List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }
    }

    public static class City
    {
        public static string[] All = new string[]
        {
            "An Giang",
            "Kon Tum",
            "Bà Rịa – Vũng Tàu",
            "Lai Châu",
            "Bắc Giang",
            "Lâm Đồng",
            "Bắc Kạn",
            "Lạng Sơn",
            "Bạc Liêu",
            "Lào Cai",
            "Bắc Ninh",
            "Long An",
            "Bến Tre",
            "Nam Định",
            "Bình Định",
            "Nghệ An",
            "Bình Dương",
            "Ninh Bình",
            "Bình Phước",
            "Ninh Thuận",
            "Bình Thuận",
            "Phú Thọ",
            "Cà Mau",
            "Phú Yên",
            "Cần Thơ",
            "Quảng Bình",
            "Cao Bằng",
            "Quảng Nam",
            "Đà Nẵng",
            "Quảng Ngãi",
            "Đắk Lắk",
            "Quảng Ninh",
            "Đắk Nông",
            "Quảng Trị",
            "Điện Biên",
            "Sóc Trăng",
            "Đồng Nai",
            "Sơn La",
            "Đồng Tháp",
            "Tây Ninh",
            "Gia Lai",
            "Thái Bình",
            "Hà Giang",
            "Thái Nguyên",
            "Hà Nam",
            "Thanh Hóa",
            "Hà Nội",
            "Thừa Thiên Huế",
            "Hà Tĩnh",
            "Tiền Giang",
            "Hải Dương",
            "TP Hồ Chí Minh",
            "Hải Phòng",
            "Trà Vinh",
            "Hậu Giang",
            "Tuyên Quang",
            "Hòa Bình",
            "Vĩnh Long",
            "Hưng Yên",
            "Vĩnh Phúc",
            "Khánh Hòa",
            "Yên Bái",
            "Kiên Giang",
        }.OrderBy(c => c).ToArray();
    }

    public static class DateTimeExtensions
    {

        public static DateTime ToVNDateTime(this DateTime utcDate)
        {
            TimeZoneInfo sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, sourceTimeZone);
            return localDate;
        }

    }

    public static class CultureViewHelper
    {
        public static string SelectedLang(CultureInfo lang)
        {
            return CultureHelper.CurrentLang.Equals(lang.TwoLetterISOLanguageName,
                StringComparison.InvariantCultureIgnoreCase) ? "selected" : "";
        }

        public static string DisplayStyle(string lang)
        {
            return CultureHelper.CurrentLang.Equals(lang, StringComparison.InvariantCultureIgnoreCase) ?
                "" : "display:none;";
        }
    }

    public static class CultureHelper
    {
        public static string CurrentLang
        {
            get
            {
                return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            }
        }

        public static IDictionary<string, string> ToMultilangString(this string val)
        {
            try
            {
                var dict = JsonConvert.DeserializeObject<IDictionary<string, string>>(val);
                return dict;
            }
            catch (Exception)
            {
                return new Dictionary<string, string>()
                {
                    {"",val }
                };
            }
        }

        public static IDictionary<string, string> ToMultilangString(this string[] vals)
        {
            var dict = new Dictionary<string, string>();
            var supp = App.Instance.SupportedCultures;
            for (var i = 0; i < vals.Length; i++)
                dict[supp[i]] = vals[i];
            return dict;
        }

        public static string ToJson(this IDictionary<string, string> obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string GetCurrentLangString(this IDictionary<string, string> multilangVals)
        {
            var currentLang = CurrentLang;
            if (multilangVals.ContainsKey(currentLang))
                return multilangVals[currentLang];
            return multilangVals.FirstOrDefault().Value;
        }

        public static string GetLangString(this IDictionary<string, string> multilangVals, string lang)
        {
            if (multilangVals.ContainsKey(lang))
                return multilangVals[lang];
            return multilangVals.FirstOrDefault().Value;
        }

        public static string GetCurrentLangString(this string multiLangString)
        {
            try
            {
                return GetCurrentLangString(
                    JsonConvert.DeserializeObject<IDictionary<string, string>>(multiLangString));
            }
            catch (Exception)
            {
                return multiLangString;
            }
        }

    }

}
