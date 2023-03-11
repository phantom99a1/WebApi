using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace SM.Training.Utils
{
    public static class Utility
    {
        private const int LINK_DURATION = 1800;  //30minute Thời gian hiệu lực của 1 link. Đơn vị tính bằng giây
        private const string DATE_FORMAT = "dd/MM/yyyy";
        private const string DATETIME_FORMAT = "yyyyMMdd_HHmmss";
        #region Validation

        public static bool ValidateEmailAddress(string email)
        {
            bool isValid = System.Text.RegularExpressions.Regex.IsMatch(email,
              @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            return isValid;
        }

        #endregion

        #region Getting value methods

        public static T GetDictionaryKeyByValue<T>(Dictionary<T, string> dicData, string value)
        {
            var item = dicData.FirstOrDefault(c => string.Equals(c.Value, value, StringComparison.OrdinalIgnoreCase));
            return item.Key;
        }

        public static T1 GetDictionaryKeyByValue<T1, T2>(Dictionary<T1, T2> dicData, T2 value)
        {
            var item = dicData.FirstOrDefault(c => string.Equals(c.Value.ToString(), value.ToString(), StringComparison.OrdinalIgnoreCase));
            return item.Key;
        }

        public static int GetDictionaryKeyByValue(Dictionary<int, string> dicData, string value)
        {
            var item = dicData.FirstOrDefault(c => string.Equals(c.Value, value, StringComparison.OrdinalIgnoreCase));
            return item.Key;
        }

        public static string GetDictionaryValue(Dictionary<bool, string> dctName, int? sTATUS)
        {
            throw new NotImplementedException();
        }

        public static bool GetDictionaryKeyByValue(Dictionary<bool, string> dicData, string value)
        {
            var item = dicData.FirstOrDefault(c => string.Equals(c.Value, value, StringComparison.OrdinalIgnoreCase));
            return item.Key;
        }

        public static int? GetObjectInt(object value)
        {
            if (value == null)
                return null;
            int? intValue = value as int?;
            if (intValue == null)
                return null;
            return intValue;
        }

        public static string CleanString(string dirtyString)
        {
            if (dirtyString == null || dirtyString.Trim() == string.Empty)
                return string.Empty;
            else
            {
                dirtyString = dirtyString.Trim();
                return System.Text.RegularExpressions.Regex.Replace(dirtyString, @"\s+", " ");
            }
        }

        public static string GetDateString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString(DATE_FORMAT);
        }

        public static string GetDateString(DateTime? value, string format)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString(format);
        }

        public static string GetStatusByDictionary(Dictionary<int?, string> dct, object value)
        {
            if (value == null)
                return string.Empty;
            int? key = value as int?;
            if (key == null)
                return string.Empty;
            if (key.HasValue && dct.ContainsKey(key.Value))
                return dct[key];
            else
                return null;
        }

        public static string GetStatusByDictionary(Dictionary<bool?, string> dct, object value)
        {
            if (value == null)
                return string.Empty;
            bool? key = value as bool?;
            if (key == null)
                return string.Empty;
            if (key.HasValue && dct.ContainsKey(key.Value))
                return dct[key];
            else
                return null;
        }

        public static string GetDateMinuteString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("dd/MM/yyyy HH:mm");
        }

        public static string GetMinuteDateString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("HH:mm dd/MM/yyyy");
        }

        public static string GetDateTimeString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("dd/MM/yyyy hh:mm:ss");
        }

        public static string GetTimeString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("HH:mm");
        }

        public static string GetMonthString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("MM/yyyy");
        }

        public static string GetDateTimeStringWithFormat(DateTime? value, string format = "dd/MM/yyyy hh:mm:ss")
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString(format);
        }

        public static string GetString(int? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.ToString();
        }

        public static string GetString(decimal? value, int digitNumber = 2)
        {
            if (value == null)
                return string.Empty;
            else
                //return String.Format("{0:N2}", value.Value);
                return SoftMart.Core.Utilities.Utility.GetDecimalString(value.Value, digitNumber);
        }

        public static string GetString(double? value, int digitNumber = 2)
        {
            if (value == null)
                return string.Empty;
            else
                return SoftMart.Core.Utilities.Utility.GetDoubleString(value.Value, digitNumber);
        }

        public static string GetString(Single? value)
        {
            if (value == null)
                return string.Empty;
            else
            {
                decimal temp = (decimal)value;
                //return String.Format("{0:N2}", value.Value);
                return SoftMart.Core.Utilities.Utility.GetDecimalString(temp);
            }
        }

        public static string GetString(bool? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value ? "True" : "False";
        }

        //tra về tiền định dạng 2,000.00 
        //cho nghiệp vụ kế toán áp dụng cho bản in hạch toán
        public static string Format_Currency_ForAccounting(decimal amount, int digitNumber = 0)
        {
            string result = string.Empty;
            try
            {
                if (digitNumber == 0)
                {
                    result = amount.ToString("N0");
                }
                else
                {
                    int zezo = 0;
                    result = string.Format("{0}.{1}", amount.ToString("N0"), zezo.ToString("D" + digitNumber));
                }
            }
            catch (Exception)
            { }
            return result;
        }

        public static string JoinBitwiseString(Dictionary<int, string> dct, int value, string sperator = ", ")
        {
            if (dct == null) return null;

            var result = new List<string>();
            foreach (KeyValuePair<int, string> item in dct)
            {
                if ((item.Key & value) == item.Key)
                    result.Add(item.Value);
            }

            return string.Join(sperator, result);
        }

        #endregion

        #region Conversion methods

        public static bool? GetNullableBool(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            bool bValue;
            if (bool.TryParse(value, out bValue))
                return bValue;

            return null;
        }

        public static int GetInt(string value)
        {
            return int.Parse(value);
        }

        public static int? GetNullableInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            int intValue;
            if (int.TryParse(value, out intValue))
                return intValue;

            return null;
        }

        public static long? GetNullableLong(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            long longValue;
            if (long.TryParse(value, out longValue))
                return longValue;

            return null;
        }

        public static Decimal? GetNullableDecimal(string value)
        {
            return SoftMart.Core.Utilities.Utility.GetNullableDecimal(value);
        }

        public static Double? GetNullableDouble(string value)
        {
            double db;
            bool isValid = Double.TryParse(value, out db);
            if (!string.IsNullOrEmpty(value) && isValid)
            {
                return db;
            }
            else
                return null;
        }

        public static DateTime? GetNullableDate(string value)
        {
            DateTime dt;
            bool isValid = DateTime.TryParseExact(value, DATE_FORMAT, null, System.Globalization.DateTimeStyles.None, out dt);
            if (!string.IsNullOrEmpty(value) && isValid)
            {
                return dt;
            }
            else
                return null;
        }

        public static DateTime? GetNullableDate(string value, string format)
        {
            DateTime dt;
            bool isValid = DateTime.TryParseExact(value, format, null, System.Globalization.DateTimeStyles.None, out dt);
            if (!string.IsNullOrEmpty(value) && isValid)
            {
                return dt;
            }
            else
                return null;
        }

        public static decimal Get0IfNullDecimal(decimal? value)
        {
            if (value == null)
                return 0;
            else
                return value.Value;
        }

        /// <summary>
        /// Lấy giá trị Date từ Excel theo format: dd.MM.yyyy
        /// VD: 05.11.2017
        /// </summary>
        /// <param name="value">"05.11.2017"</param>
        /// <returns></returns>
        public static DateTime? GetDateFromExcelImport(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            value = value.Replace(" ", string.Empty);
            value = value.Replace("'", string.Empty);

            string[] dateFormats = new string[]
            {
                "dd/MM/yyyy",
                "dd.MM.yyyy",
                "d.MM.yyyy",
                "dd.M.yyyy",
                "d.M.yyyy",
            };

            DateTime dt;
            bool isValid = DateTime.TryParseExact(value, dateFormats, null, System.Globalization.DateTimeStyles.None, out dt);

            if (isValid)
            {
                return dt;
            }
            else
                return null;
        }

        public static DateTime? GetNullableDateTime(string value, string format = "dd/MM/yyyy hh:mm:ss")
        {
            DateTime dt;
            bool isValid = DateTime.TryParseExact(value, format, null, System.Globalization.DateTimeStyles.None, out dt);
            if (!string.IsNullOrEmpty(value) && isValid)
            {
                return dt;
            }
            else
                return null;
        }

        public static string NullIfEmptyString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            else
                return value.Trim();
        }

        public static string GetUrl(string documentUrl, string absoluteUri, string pathQuery)
        {
            //Lay Url ca query, localhost:1206/UI/Anouncements/Display?id=7 

            //Lay root url, localhost:1206
            absoluteUri = absoluteUri.Replace(pathQuery, "");

            if (string.IsNullOrEmpty(documentUrl))
                return absoluteUri;

            documentUrl = documentUrl.Replace(@"\", "/");
            return string.Format("{0}{1}", absoluteUri, documentUrl);
        }

        public static TimeSpan? GetTimeSpan(string input)
        {
            TimeSpan tmpTime;
            if (TimeSpan.TryParse(input, out tmpTime))
            {
                return tmpTime;
            }
            return null;
        }
        #endregion

        #region Dictionary utils

        public static string GetDictionaryValue<T>(Dictionary<T, string> dicInput, T key)
        {
            if (key != null && dicInput.ContainsKey(key))
                return dicInput[key];

            return string.Empty;
        }

        public static T2 GetDictionaryValue<T1, T2>(Dictionary<T1, T2> dicInput, T1 key)
        {
            if (key != null && dicInput.ContainsKey(key))
                return (T2)dicInput[key];

            return default(T2);
        }

        public static string GetDictionaryValue(Dictionary<int, string> dicInput, int? key)
        {
            if (key != null && dicInput.ContainsKey(key.Value))
                return dicInput[key.Value];

            return string.Empty;
        }

        public static string GetDictionaryValue(Dictionary<bool, string> dicInput, bool? key)
        {
            if (key != null && dicInput.ContainsKey(key.Value))
                return dicInput[key.Value];

            return string.Empty;
        }

        public static int? GetDictionaryValue(Dictionary<int, int> dicInput, int? key)
        {
            if (key != null && dicInput.ContainsKey(key.Value))
                return dicInput[key.Value];

            return 0;
        }

        public static int GetDictionaryValue(Dictionary<string, int> dicInput, string key)
        {
            if (key != null && dicInput.ContainsKey(key))
                return dicInput[key];

            return 0;
        }

        public static T GetKeyByValueDictionary<T>(Dictionary<T, string> dicInput, string value)
        {
            if (!string.IsNullOrEmpty(value))
                return dicInput.FirstOrDefault(d => d.Value.Equals(value)).Key;

            return default(T);
        }

        #endregion

        /// <summary>
        /// Lấy folder hiện tại của ứng dụng
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyFolder()
        {
            string assemblyFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return assemblyFolder;
        }

        public static string CatChuoi(string chuoi, int length)
        {
            if (string.IsNullOrEmpty(chuoi))
            {
                return string.Empty;
            }
            if (chuoi.Length > length)
            {
                return chuoi.Substring(0, length - 3) + "..";
            }
            else
            {
                return chuoi;
            }
        }

        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static string RemoveXSS(string dirtyString)
        {
            if (string.IsNullOrWhiteSpace(dirtyString))
            {
                return dirtyString;
            }
            else
            {
                Regex rg = new Regex("<.*?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string result = rg.Replace(dirtyString, string.Empty);

                rg = new Regex("javascript:", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                result = rg.Replace(result, string.Empty);
                return result;
            }
        }

        /// <summary>
        /// Ex: number 4: 155000 -> 160000;154999 -> 150000;
        /// </summary>
        public static string GetRoundedString(decimal? value, int number = 4)
        {
            if (value == null)
                return null;

            decimal padding = (decimal)Math.Pow(10, number);
            Decimal d = Decimal.Divide(value.Value, padding);
            d = decimal.Round(d);
            d = d * padding;
            return d.ToString("N0");
        }

        /// <summary>
        /// Ex: number 4: 155000 -> 160000;154999 -> 150000;
        /// </summary>
        public static decimal? GetRoundedDecimal(decimal? value, int number = 4)
        {
            if (value == null)
                return null;

            decimal padding = (decimal)Math.Pow(10, number);
            Decimal d = Decimal.Divide(value.Value, padding);
            d = decimal.Round(d);
            d = d * padding;
            return d;
        }
        #region Convert number 2 word
        public static string GetWordNumber(decimal? number)
        {
            if (number == null)
                return string.Empty;

            decimal positiveNumber = Math.Abs(number.Value);
            string strNumber = positiveNumber.ToString("#.00");
            string[] arrNumber = strNumber.Split(new char[] { '.', ',' });

            string word = GetWordNumber_ReadNumber(arrNumber[0]);
            string strScale = GetWordNumber_ReadNumber(arrNumber[1]);

            if (!string.IsNullOrWhiteSpace(strScale))
                word = word + " phẩy " + strScale;
            if (number < 0)
                word = "âm " + word;
            if (number == 0)
                word = "Không";

            if (word.Length > 0)
                return word[0].ToString().ToUpper() + word.Substring(1);
            else
                return string.Empty;
        }

        private static string GetWordNumber_ReadNumber(string number)
        {
            string word = string.Empty;
            Dictionary<int, string> dicUnitName = new Dictionary<int, string>() { { 0, "tỷ" }, { 1, "nghìn" }, { 2, "triệu" } };
            List<string> lstUnit3 = GetWordNumber_GroupByLength(number, 3);
            string sperateWord = string.Empty;

            for (int index = 0; index < lstUnit3.Count; index++)
            {
                string unit = lstUnit3[index];
                string unitWord = GetWordNumber_ReadThousand(unit);

                int remainIndex = lstUnit3.Count - index - 1;
                int unitIndex = remainIndex % 3;
                string unitUnit = (remainIndex == 0) ? string.Empty : dicUnitName[unitIndex];

                if (!string.IsNullOrWhiteSpace(unitWord))
                {
                    if (!string.IsNullOrWhiteSpace(unitUnit))
                        unitWord = unitWord + " " + unitUnit;

                    word = word + sperateWord + unitWord;
                    sperateWord = " ";
                }

                // truong hop don vi Ty ma khong co so doc dang sau (vd: 23 000 000 000 000)
                if (unitIndex == 0 && (remainIndex / 3) > 0 && string.IsNullOrWhiteSpace(unitWord))
                    word = word + " tỷ";
            }

            word = word.Trim();
            if (!string.IsNullOrWhiteSpace(word) && word.Length > 0 && char.IsLower(word[0]))
                word = char.ToUpper(word[0]).ToString() + word.Substring(1);

            return word;
        }

        private static List<string> GetWordNumber_GroupByLength(string number, int length)
        {
            List<string> lstUnit = new List<string>();
            string unit = string.Empty;
            for (int index = 0; index < number.Length; index++)
            {
                int remainIndex = number.Length - index;
                if (index > 0 && (remainIndex % length == 0))
                {
                    lstUnit.Add(unit);
                    unit = string.Empty;
                }

                unit = unit + number[index].ToString();
            }
            if (!string.IsNullOrWhiteSpace(unit))
                lstUnit.Add(unit);

            return lstUnit;
        }

        private static string GetWordNumber_ReadThousand(string number)
        {
            Dictionary<char, string> dicNumber = new Dictionary<char, string>()
            {
                {'0', "không"}, {'1', "một"}, {'2', "hai"}, {'3', "ba"}, {'4', "bốn"},
                {'5', "năm"}, {'6', "sáu"}, {'7', "bảy"}, {'8', "tám"}, {'9', "chín"}
            };

            switch (number.Length)
            {
                case 1:
                case 2:
                    return GetWordNumber_ReadHundred(number);
                case 3:
                    if (number == "000")
                        return string.Empty;

                    return dicNumber[number[0]] + " trăm " + GetWordNumber_ReadHundred(number.Substring(1));
                default:
                    return string.Empty;
            }
        }

        private static string GetWordNumber_ReadHundred(string number)
        {
            Dictionary<char, string> dicNumber = new Dictionary<char, string>()
            {
                {'0', "lẻ"}, {'1', "một"}, {'2', "hai"}, {'3', "ba"}, {'4', "bốn"},
                {'5', "năm"}, {'6', "sáu"}, {'7', "bảy"}, {'8', "tám"}, {'9', "chín"}
            };
            Dictionary<char, string> dicNumber2 = new Dictionary<char, string>()
            {
                {'0', "lẻ"}, {'1', "một"}, {'2', "hai"}, {'3', "ba"}, {'4', "bốn"},
                {'5', "lăm"}, {'6', "sáu"}, {'7', "bảy"}, {'8', "tám"}, {'9', "chín"}
            };

            switch (number.Length)
            {
                case 1:
                    if (number[0] == '0')
                        return string.Empty;

                    return dicNumber[number[0]];
                case 2:
                    if (number == "00")
                        return string.Empty;
                    if (number == "10")
                        return "mười";
                    if (number[1] == '0')
                        return dicNumber[number[0]] + " mươi";
                    if (number[0] == '1')
                        return "mười " + dicNumber2[number[1]];
                    if (number[0] == '0')
                        return dicNumber[number[0]] + " " + dicNumber[number[1]];

                    return dicNumber[number[0]] + " mươi " + dicNumber2[number[1]];
                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}