using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ModernUINavigationApp1
{
    class SimpleJSON
    {
        static Dictionary<string, string> res;
        /// <summary>Парсим JSON строку (не простое число) и возвращаем одноуровневый список значений с именами полей, собираемые из имен json параметров и элементов массивов</summary>
        /// <param name="str">json строка</param>
        /// <returns>Возвращаем одноуровневый массив всех пар имя=>значение, где имя состоит из вложенных имен значений из json разделеных точками, например orders[0].amount=>10</returns>
        static public Dictionary<string, string> pairsParseJSON(string str)
        {
            res = new Dictionary<string, string>();
            SimpleJSON.parsePair(str.Trim(new char[] { '\n', '\r', '\t', ' ' }), 0, 0, "");
            return res;
        }
        static int parsePair(string str, int pos, int lvl, string names)
        {
            int strlen = str.Length;
            int idx = -1;
            while (pos < strlen)
            { // на кажэдом шаге мы начинаем либо с символов начала списка или объекта, либо с элемента списка, которвй в свою очередь либо элемент либо именованный элемент
                idx++;
                // пробелы и запятые
                while (pos < strlen && (str[pos] == ' ' || str[pos] == '\t' || str[pos] == '\n' || str[pos] == '\r' || str[pos] == ',')) pos++;
                // именованный параметр: "test":
                Regex re = new Regex("^\"(([^\"\\\\]+)(\\\\.[^\"\\\\]*)*)\" *: *");
                Match rm = re.Match(str.Substring(pos));
                string lname;
                if (!rm.Success)
                { // нет имени, значит это элемент массива, берем предыдущий, и прибавляем к нему 1, если это первый - имя равно 0
                    // длинное имя
                    lname = (names != "" ? names + "[" : "") + (lvl == 0 ? "" : idx.ToString()) + (names != "" ? "]" : "");
                }
                else
                {
                    string name = rm.Groups[1].Value.Trim('"');
                    pos += rm.Length;
                    // длинное имя
                    lname = (names != "" ? names + "." : "") + (lvl == 0 ? "" : name);
                }
                // пробелы
                while (pos < strlen && (str[pos] == ' ' || str[pos] == '\t' || str[pos] == '\n' || str[pos] == '\r')) pos++;
                // проверим начало списка или объекта {, [
                if (str[pos] == '{' || str[pos] == '[') { pos = SimpleJSON.parsePair(str, pos + 1, lvl + 1, lname); continue; }
                // проверим окончание списка ии объекта }, ]
                if (str[pos] == '}' || str[pos] == ']') return pos + 1;
                // простые значения
                if (str[pos] == '"') re = new Regex("\"([^\"\\\\]+(\\\\.[^\"\\\\]*)*)\""); else re = new Regex("([^, \\:}\\]]+)");
                rm = re.Match(str, pos);
                if (!rm.Success) return pos + 1; // todo: обработать корректно ошибки
                res[lname] = rm.Value.Trim('"');
                pos += rm.Length;
                //
            }
            return pos;
        }

        static public Dictionary<string, object> treeParseJSON(string str)
        {
            int pos = 0;
            Dictionary<string, object> res = SimpleJSON.parseTree(str.Trim(new char[] { '\n', '\r', '\t', ' ' }), ref pos, 0);
            if (res.Count == 1 && res.ContainsKey("0") && res["0"].GetType() == typeof(Dictionary<string, object>)) return (Dictionary<string, object>)res["0"];
            return res;
        }
        /// <summary>Сканирует строку str в позиции pos на наличие элемента список-json ивозвращает в виде объекта tree</summary>
        /// <param name="str"></param>
        /// <param name="pos"></param>
        /// <param name="lvl"></param>
        /// <returns></returns>
        static Dictionary<string, object> parseTree(string str, ref int pos, int lvl)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            int strlen = str.Length;
            int idx = -1; // начинаем с 0, но так как шаг увеличивается в начале списка... то -1
            while (pos < strlen)
            { // на кажэдом шаге мы начинаем либо с символов начала списка или объекта, либо с элемента списка, которвй в свою очередь либо элемент либо именованный элемент
                idx++;
                // пробелы и запятые
                while (pos < strlen && (str[pos] == ' ' || str[pos] == '\t' || str[pos] == '\n' || str[pos] == '\r' || str[pos] == ',')) pos++;
                if (pos >= strlen) break;
                // именованный параметр: "test":
                Regex re = new Regex("^\"(([^\"\\\\]+)(\\\\.[^\"\\\\]*)*)\" *: *");
                Match rm = re.Match(str.Substring(pos));
                string name;
                if (!rm.Success)
                { // нет имени, значит это элемент массива
                    // длинное имя
                    name = idx.ToString();
                }
                else
                {
                    name = rm.Groups[1].Value.Trim('"');
                    pos += rm.Length;
                }
                // пробелы
                while (pos < strlen && (str[pos] == ' ' || str[pos] == '\t' || str[pos] == '\n' || str[pos] == '\r')) pos++;
                // проверим начало списка или объекта {, [
                if (str[pos] == '{' || str[pos] == '[') { pos++; res[name] = SimpleJSON.parseTree(str, ref pos, lvl + 1); continue; }
                // проверим окончание списка ии объекта }, ]
                if (str[pos] == '}' || str[pos] == ']') { pos++; return res; }
                // простые значения
                bool isNumber = true;
                if (str[pos] == '"') { re = new Regex("\"([^\"\\\\]*(\\\\.[^\"\\\\]*)*)\""); isNumber = false; } else re = new Regex("([^, \\:}\\]]+)");
                rm = re.Match(str, pos);
                if (!rm.Success) { pos++; return res; } // todo: обработать корректно ошибки
                if (isNumber && new Regex(@"^[0-9]+$").Match(rm.Value).Success) res[name] = Convert.ToInt64(rm.Value); else res[name] = rm.Value.Trim('"');
                pos += rm.Length;
                //
            }
            return res;
        }
        /// <summary>формируем строку json типа строка, обрамляя ковычками и подставляя коды символов \xXXXX</summary>
        /// <param name="elem">строка</param>
        /// <returns>строка в формате json</returns>
        public static string str2json(string elem)
        { // todo: сделать корректное преобразование для любых символов
            return "\"" + elem.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t") + "\"";
        }
        /// <summary>формируем строку json на основе списка типа tree</summary>
        /// <param name="tree">список, элементы либо dictionary<string,object> либо object</param>
        /// <returns>строка в формате json</returns>
        public static string makeJson(Dictionary<string, object> tree)
        {
            // определим, массив это или объект
            int idx = 0;
            bool isArray = true;
            foreach (KeyValuePair<string, object> o in tree) if (o.Key != (idx++).ToString()) { isArray = false; break; }
            if (isArray) return "[" + makeJsonList(tree, true) + "]"; else return "{" + makeJsonList(tree, false) + "}";
        }
        public static string makeJson(Dictionary<string, string> list)
        {
            Dictionary<string, object> tree = new Dictionary<string, object>();
            foreach (KeyValuePair<string, string> o in list) tree.Add(o.Key, o.Value);
            return makeJson(tree);
        }
        /// <summary>формируем строку, состояющую из элементов json через запятую</summary>
        static string makeJsonList(Dictionary<string, object> tree, bool skipNames)
        {
            string res = "";
            foreach (KeyValuePair<string, object> o in tree) if (o.Value.GetType() == typeof(Dictionary<string, object>))
                { // другой список
                    res += (res == "" ? "" : ", ") + (skipNames ? "" : SimpleJSON.str2json(o.Key) + ":") + SimpleJSON.makeJson((Dictionary<string, object>)o.Value);
                }
                else
                { // простой элемент - число, строка
                    if (o.Value.GetType() == typeof(double)
                     || o.Value.GetType() == typeof(Double)
                     || o.Value.GetType() == typeof(float)
                     || o.Value.GetType() == typeof(int)
                     || o.Value.GetType() == typeof(Int16)
                     || o.Value.GetType() == typeof(Int32)
                     || o.Value.GetType() == typeof(Int64)
                     || o.Value.GetType() == typeof(long))
                        res += (res == "" ? "" : ", ") + (skipNames ? "" : SimpleJSON.str2json(o.Key) + ":") + o.Value.ToString().Replace(',', '.');
                    else res += (res == "" ? "" : ", ") + (skipNames ? "" : SimpleJSON.str2json(o.Key) + ":") + SimpleJSON.str2json(o.Value.ToString());
                }
            return res;
        }
        public static double ConvertToDouble(string str)
        {
            return Convert.ToDouble(str.Replace(" ", "").Replace(".", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));
        }
    }
}
