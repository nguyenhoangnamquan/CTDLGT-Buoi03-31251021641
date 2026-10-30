using System;
using System.Diagnostics;

namespace CircularQueueImplementation
{
    // Cài đặt lớp Hàng đợi vòng CircularQueue<T> bằng mảng tĩnh theo đúng giao diện yêu cầu
    public class CircularQueue<T>
    {
        private T[] items;    // Mảng lưu trữ các phần tử dữ liệu
        private int front;    // Chỉ số quản lý phần tử ở đầu hàng đợi
        private int count;    // Biến lưu số lượng phần tử hiện đang có trong hàng đợi

        // Thuộc tính chỉ đọc trả về số lượng phần tử hiện tại
        public int Count => count;

        // Thu thuộc tính chỉ đọc trả về dung lượng mảng hiện tại
        public int Capacity => items.Length;

        // Khởi tạo hàng đợi với dung lượng ban đầu là 4 theo đúng ràng buộc bài toán
        public CircularQueue(int initialCapacity = 4)
        {
            items = new T[initialCapacity];
            front = 0;
            count = 0;
        }

        // Thêm phần tử vào cuối hàng đợi; tự động mở rộng mảng gấp đôi nếu mảng đã đầy
        public void Enqueue(T value)
        {
            // Kiểm tra mảng đã đạt sức chứa tối đa hay chưa để tiến hành mở rộng
            if (count == Capacity)
            {
                Resize(Capacity * 2);
            }

            // Tính vị trí rear để chèn phần tử mới theo công thức quay vòng (front + count) % Capacity
            int rear = (front + count) % Capacity;
            items[rear] = value;

            // Tăng số lượng phần tử hiện có trong hàng đợi
            count++;
        }

        // Lấy và xóa phần tử ở đầu hàng đợi
        public T Dequeue()
        {
            // Kiểm tra hàng đợi rỗng để ném InvalidOperationException theo đúng quy định
            if (count == 0)
            {
                throw new InvalidOperationException("Hàng đợi đang rỗng, không thể thực hiện Dequeue.");
            }

            // Lấy giá trị phần tử ở đầu hàng đợi
            T value = items[front];

            // Đặt ô vừa lấy về default để giải phóng tham chiếu cũ cho bộ thu gom rác (GC), dấu ! bỏ qua cảnh báo null
            items[front] = default!;

            // Dịch chuyển chỉ số front sang vị trí kế tiếp theo cơ chế quay vòng (% Capacity)
            front = (front + 1) % Capacity;

            // Giảm số lượng phần tử trong hàng đợi
            count--;

            return value;
        }

        // Xem phần tử ở đầu hàng đợi mà không xóa
        public T Peek()
        {
            // Kiểm tra hàng đợi rỗng để ném InvalidOperationException theo đúng quy định
            if (count == 0)
            {
                throw new InvalidOperationException("Hàng đợi đang rỗng, không thể thực hiện Peek.");
            }

            // Trả về phần tử nằm ở vị trí front hiện tại
            return items[front];
        }

        // Hàm hỗ trợ mở rộng dung lượng mảng và chép dữ liệu theo đúng thứ tự logic
        private void Resize(int newCapacity)
        {
            T[] newItems = new T[newCapacity];

            // Chép n phần tử từ mảng cũ sang mảng mới theo thứ tự logic tính từ front
            for (int i = 0; i < count; i++)
            {
                // Vị trí phần tử thứ i trong mảng cũ được tính bằng công thức quay vòng (front + i) % Capacity
                newItems[i] = items[(front + i) % Capacity];
            }

            // Gán lại mảng mới và đưa con trỏ front về lại chỉ số 0 chuẩn ban đầu
            items = newItems;
            front = 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            //  STT 1: Hàng rỗng 
            Console.WriteLine("Ca 1");
            var q1 = new CircularQueue<string>();
            Console.WriteLine($"  Count: {q1.Count}, Capacity: {q1.Capacity}");
            try { q1.Dequeue(); }
            catch (InvalidOperationException ex) { Console.WriteLine($"  Dequeue lỗi expected: {ex.Message}"); }
            try { q1.Peek(); }
            catch (InvalidOperationException ex) { Console.WriteLine($"  Peek lỗi expected: {ex.Message}"); }
            Console.WriteLine();

            //  STT 2: Vừa đầy 
            Console.WriteLine("Ca 2");
            var q2 = new CircularQueue<string>();
            q2.Enqueue("A"); q2.Enqueue("B"); q2.Enqueue("C"); q2.Enqueue("D");
            Console.WriteLine($"  Count: {q2.Count}, Capacity: {q2.Capacity}, Peek: {q2.Peek()}");
            Console.WriteLine();

            //  STT 3: Quay vòng chỉ số, chưa mở rộng 
            Console.WriteLine("Ca 3");
            var q3 = new CircularQueue<string>();
            q3.Enqueue("A"); q3.Enqueue("B"); q3.Enqueue("C"); q3.Enqueue("D");
            string d1 = q3.Dequeue(); string d2 = q3.Dequeue();
            Console.WriteLine($"  Lấy ra 2 lần đầu: {d1}, {d2}");
            q3.Enqueue("E"); q3.Enqueue("F");
            Console.WriteLine($"  Capacity hiện tại: {q3.Capacity}");
            Console.Write("  Lấy ra 4 lần tiếp: ");
            for (int i = 0; i < 4; i++) Console.Write($"{q3.Dequeue()} ");
            Console.WriteLine("\n");

            //  STT 4: Mở rộng khi front = 0 
            Console.WriteLine("Ca 4");
            var q4 = new CircularQueue<int>();
            for (int i = 1; i <= 5; i++) q4.Enqueue(i);
            Console.WriteLine($"  Capacity sau khi thêm 5 phần tử: {q4.Capacity}");
            Console.Write("  Lấy ra 5 lần: ");
            while (q4.Count > 0) Console.Write($"{q4.Dequeue()} ");
            Console.WriteLine("\n");

            //  STT 5: Mở rộng khi front khác 0 
            Console.WriteLine("Ca 5");
            var q5 = new CircularQueue<string>();
            q5.Enqueue("A"); q5.Enqueue("B"); q5.Enqueue("C"); q5.Enqueue("D");
            q5.Dequeue(); q5.Dequeue(); // front = 2
            q5.Enqueue("E"); q5.Enqueue("F"); q5.Enqueue("G"); // Kích hoạt Resize khi front khác 0
            Console.WriteLine($"  Capacity sau khi mở rộng: {q5.Capacity}");
            Console.Write("  Lấy ra 5 phần tử đúng thứ tự: ");
            while (q5.Count > 0) Console.Write($"{q5.Dequeue()} ");
            Console.WriteLine("\n");

            //  STT 6: Quay vòng rất nhiều lần 
            Console.WriteLine("Ca 6");
            var q6 = new CircularQueue<int>();
            for (int i = 0; i < 1000000; i++)
            {
                q6.Enqueue(i);
                q6.Dequeue();
            }
            Console.WriteLine($"  Chạy thành công 1,000,000 lần! Capacity cuối: {q6.Capacity}, Count: {q6.Count}");
            Console.WriteLine();

            //  STT 7: Ca lớn, nhiều lần mở rộng 
            Console.WriteLine("Ca 7");
            var q7 = new CircularQueue<int>();
            int largeCount = 100000;
            var sw = Stopwatch.StartNew();

            for (int i = 1; i <= largeCount; i++) q7.Enqueue(i);
            int capacityAfterEnq = q7.Capacity;

            bool isOrderCorrect = true;
            for (int i = 1; i <= largeCount; i++)
            {
                if (q7.Dequeue() != i)
                {
                    isOrderCorrect = false;
                    break;
                }
            }
            sw.Stop();

            Console.WriteLine($"  Lấy ra đúng thứ tự: {isOrderCorrect}");
            Console.WriteLine($"  Capacity cuối: {capacityAfterEnq}");
            Console.WriteLine($"  [Thời gian thực thi]: {sw.ElapsedMilliseconds} ms");
        }
    }
}