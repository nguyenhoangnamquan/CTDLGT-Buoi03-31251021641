using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BalancedBracketsCheck
{
    class Program
    {
        // Hàm kiểm tra tính hợp lệ của chuỗi ngoặc và trả về kết quả (isValid, errorPos)
        public static (bool isValid, int errorPos) CheckBalancedBrackets(string s)
        {
            // Dùng Stack lưu cặp (ký tự ngoặc mở, chỉ số) để vừa đối chiếu kiểu ngoặc vừa truy vết vị trí lỗi.
            var stack = new Stack<(char ch, int pos)>();

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];

                // Lưu vị trí và loại ngoặc mở vào Stack để chờ ngoặc đóng phù hợp ở phía sau.
                if (c == '(' || c == '[' || c == '{')
                {
                    stack.Push((c, i));
                }
                // Khi gặp ngoặc đóng, kiểm tra tính khớp với ngoặc mở gần nhất trên đỉnh Stack.
                else if (c == ')' || c == ']' || c == '}')
                {
                    // Ngoặc đóng xuất hiện khi Stack rỗng: Không có ngoặc mở nào trước đó để ghép cặp -> Lỗi đóng thừa tại vị trí i.
                    if (stack.Count == 0)
                    {
                        return (false, i);
                    }

                    // Lấy ngoặc mở gần nhất ra để đối chiếu loại ngoặc với ngoặc đóng hiện tại.
                    var top = stack.Pop();

                    // Ngoặc đóng hiện tại khác loại với ngoặc mở trên đỉnh Stack -> Lỗi đóng sai loại tại vị trí i.
                    if (!IsMatching(top.ch, c))
                    {
                        return (false, i);
                    }
                }
                // Các ký tự khác ngoặc sẽ tự động bị bỏ qua theo yêu cầu bài toán.
            }

            // Duyệt hết chuỗi nhưng Stack vẫn còn phần tử: Ngoặc mở không bao giờ được đóng.
            // Lấy phần tử đáy Stack (ngoặc mở đưa vào đầu tiên) để báo lỗi ở vị trí đứng sớm nhất trong chuỗi.
            if (stack.Count > 0)
            {
                return (false, stack.Last().pos);
            }

            // Chuỗi hoàn toàn cân bằng và hợp lệ.
            return (true, -1);
        }

        // Hàm hỗ trợ kiểm tra xem ngoặc mở và ngoặc đóng có thuộc cùng một loại hay không.
        private static bool IsMatching(char open, char close)
        {
            return (open == '(' && close == ')') ||
                   (open == '[' && close == ']') ||
                   (open == '{' && close == '}');
        }

        static void Main(string[] args)
        {
            RunAllTestCases();
        }

        private static void RunAllTestCases()
        {
            // Sinh dữ liệu cho hai ca kiểm thử kích thước lớn (Ca 9 và Ca 10)
            string test9Input = new string('(', 500000) + new string(')', 500000);
            string test10Input = new string('(', 1000000);

            var testCases = new (string input, string desc)[]
            {
                ("{(a+b)×[c-d]}", "Trường hợp thông thường"),
                ("{(a+b]×c)", "Đóng sai loại"),
                ("(a+b", "Mở chưa đóng"),
                ("a+b)", "Đóng thừa"),
                ("", "Ca biên (chuỗi rỗng)"),
                ("([)]", "Các cặp đan xen nhau"),
                ("a+b", "Không có ngoặc"),
                ("{[(", "Nhiều ngoặc mở chưa đóng"),
                (test9Input, "Ca lớn: 500k '(' và 500k ')'"),
                (test10Input, "Ca lớn: 1M '('")
            };

            Console.WriteLine("{0,-5} | {1,-30} | {2,-20} | {3,-15}", "STT", "Chuỗi vào (mô tả)", "Kết quả mong đợi", "Thời gian chạy");
            Console.WriteLine(new string('-', 80));

            for (int i = 0; i < testCases.Length; i++)
            {
                // Dùng Stopwatch đo chính xác thời gian thực thi cho từng ca kiểm thử.
                var sw = Stopwatch.StartNew();
                var (isValid, errorPos) = CheckBalancedBrackets(testCases[i].input);
                sw.Stop();

                string result = isValid ? "HOP LE" : $"SAI tai vi tri {errorPos}";
                string displayInput = testCases[i].input.Length > 20 
                    ? testCases[i].input.Substring(0, 15) + "..." 
                    : $"\"{testCases[i].input}\"";

                Console.WriteLine("{0,-5} | {1,-30} | {2,-20} | {3,-15}", 
                    i + 1, displayInput, result, $"{sw.ElapsedMilliseconds} ms");
            }
        }
    }
}