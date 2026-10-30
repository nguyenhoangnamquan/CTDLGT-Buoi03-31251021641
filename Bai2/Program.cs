using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace ServiceCounterSimulation
{
    // Record lưu thông tin từng khách hàng bao gồm số thứ tự, thời điểm đến và thời gian phục vụ
    public record Customer(int Id, long ArrivalTime, long ServiceTime);

    class Program
    {
        // Hàm mô phỏng quầy dịch vụ theo thuật toán Queue (FIFO)
        public static void SimulateService(int n, int k, List<(long a, long s)> inputCustomers, bool verbose = true)
        {
            // Dùng Queue để quản lý hàng chờ khách hàng theo đúng thứ tự đến (FIFO), không dùng List.RemoveAt(0)
            var queue = new Queue<Customer>();
            for (int i = 0; i < n; i++)
            {
                queue.Enqueue(new Customer(i + 1, inputCustomers[i].a, inputCustomers[i].s));
            }

            // Mảng lưu thời điểm rảnh tiếp theo của k quầy phục vụ (chỉ số quầy từ 1 đến k)
            long[] counterFreeTime = new long[k + 1];

            // Biến dùng long để chứa tổng thời gian chờ, tránh hiện tượng tràn số khi n và a lớn
            long totalWaitTime = 0;
            long maxWaitTime = 0;
            long finishAllTime = 0;

            // Xử lý lần lượt từng khách hàng trong Queue cho đến khi hàng chờ rỗng
            while (queue.Count > 0)
            {
                // Lấy khách hàng đứng đầu hàng chờ ra phục vụ với chi phí O(1)
                var cust = queue.Dequeue();

                // Thuật toán tìm quầy rảnh sớm nhất: duyệt qua k quầy để tìm thời điểm rảnh nhỏ nhất
                int chosenCounter = 1;
                long minFreeTime = counterFreeTime[1];

                for (int q = 2; q <= k; q++)
                {
                    // Chọn quầy có thời điểm rảnh sớm hơn; nếu bằng nhau thì giữ quầy có số thứ tự nhỏ hơn
                    if (counterFreeTime[q] < minFreeTime)
                    {
                        minFreeTime = counterFreeTime[q];
                        chosenCounter = q;
                    }
                }

                // Thời điểm bắt đầu phục vụ phải là thời điểm muộn hơn giữa lúc quầy rảnh và lúc khách tới
                long startTime = Math.Max(minFreeTime, cust.ArrivalTime);
                long waitTime = startTime - cust.ArrivalTime;
                long endTime = startTime + cust.ServiceTime;

                // Cập nhật lại thời điểm rảnh mới cho quầy vừa được chọn
                counterFreeTime[chosenCounter] = endTime;

                // Cập nhật các chỉ số thống kê tổng quát của hệ thống
                totalWaitTime += waitTime;
                if (waitTime > maxWaitTime)
                {
                    maxWaitTime = waitTime;
                }
                if (endTime > finishAllTime)
                {
                    finishAllTime = endTime;
                }

                // In thông tin chi tiết từng khách (tắt khi chạy ca lớn để tránh nghẽn I/O Console)
                if (verbose)
                {
                    Console.WriteLine($"  KH {cust.Id}: den {cust.ArrivalTime}, quay {chosenCounter}, bat dau {startTime}, cho {waitTime}, xong {endTime}");
                }
            }

            // In kết quả thống kê theo định dạng số thực 2 chữ số thập phân dùng CultureInfo.InvariantCulture
            double avgWaitTime = (double)totalWaitTime / n;
            Console.WriteLine($"  Cho trung binh: {avgWaitTime.ToString("F2", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"  Cho lau nhat: {maxWaitTime}");
            Console.WriteLine($"  Hoan thanh luc: {finishAllTime}");
        }

        static void Main(string[] args)
        {

            // STT 1: Trường hợp một quầy (k = 1)
            Console.WriteLine("Ca 1");
            SimulateService(3, 1, new List<(long, long)> { (0, 5), (1, 3), (2, 4) });
            Console.WriteLine();

            // STT 2: Trường hợp hai quầy (k = 2)
            Console.WriteLine("Ca 2");
            SimulateService(3, 2, new List<(long, long)> { (0, 5), (1, 3), (2, 4) });
            Console.WriteLine();

            // STT 3: Các khách hàng đến cùng lúc (k = 2)
            Console.WriteLine("Ca 3");
            SimulateService(3, 2, new List<(long, long)> { (0, 3), (0, 3), (0, 3) });
            Console.WriteLine();

            // STT 4: Quầy rảnh nghỉ một khoảng thời gian mới có khách tiếp theo đến (k = 1)
            Console.WriteLine("Ca 4");
            SimulateService(2, 1, new List<(long, long)> { (0, 2), (10, 3) });
            Console.WriteLine();

            // STT 5: Số quầy k lớn hơn số lượng khách n (n = 1, k = 3)
            Console.WriteLine("Ca 5");
            SimulateService(1, 3, new List<(long, long)> { (4, 6) });
            Console.WriteLine();

            // STT 6: Ca kiểm thử lớn (n = 100,000, k = 10)
            Console.WriteLine("Ca 6");
            int nLarge = 100000;
            int kLarge = 10;
            var largeCustomers = new List<(long a, long s)>(nLarge);
            for (int i = 0; i < nLarge; i++)
            {
                // Khách thứ 1 đến lúc 0, khách thứ 2 đến lúc 1, ..., khách thứ i đến lúc i - 1
                largeCustomers.Add((i, 10)); 
            }

            // Sử dụng Stopwatch đo chính xác thời gian chạy của ca lớn
            var sw = Stopwatch.StartNew();
            // Đặt verbose = false để không in 100,000 dòng thông tin cá nhân ra màn hình
            SimulateService(nLarge, kLarge, largeCustomers, verbose: false);
            sw.Stop();

            Console.WriteLine($"  [Thoi gian chay thuc te]: {sw.ElapsedMilliseconds} ms");
        }
    }
}