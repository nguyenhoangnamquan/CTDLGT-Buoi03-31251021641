Tên: Nguyễn Hoàng Nam Quân
MSSV: 31251021641
LHP: 26C1INF50900706
NỘI DUNG MÔ TẢ TỔNG HỢP CẢ 3 BÀI TOÁN

1. Hướng dẫn biên dịch và chạy chương trình của từng bài:
   - Môi trường yêu cầu: .NET SDK (C# 8.0 trở lên), Microsoft Visual Studio hoặc Visual Studio Code.
   - Các bước thực hiện:
     + Bước 1: Mở môi trường lập trình và tạo dự án mới dạng C# Console Application cho từng bài.
     + Bước 2: Ghi mã nguồn C# đầy đủ của bài tương ứng vào tệp Program.cs.
     + Bước 3: Mở Terminal/Command Prompt tại thư mục chứa tệp dự án và chạy lệnh "dotnet run".
     + Hàm Main của mỗi bài đã được tích hợp sẵn toàn bộ các ca kiểm thử theo yêu cầu, chương trình sẽ tự động thực thi và in rõ ràng từng kết quả kiểm thử ra màn hình console.

2. Mô tả ngắn giải pháp, độ phức tạp thời gian và bộ nhớ của từng bài:
   - Bài 1 (Kiểm tra dấu ngoặc cân bằng - Stack):
     + Giải pháp: Dùng cấu trúc Stack<(char ch, int pos)> lưu vết ngoặc mở và vị trí của nó theo cơ chế LIFO. Duyệt chuỗi từ trái sang phải, gặp ngoặc đóng thì Pop phần tử đỉnh Stack ra đối chiếu loại ngoặc. Báo lỗi đóng thừa khi Stack rỗng, đóng sai loại khi không trùng kiểu, hoặc mở chưa đóng nếu duyệt xong mà Stack còn phần tử.
     + Độ phức tạp thời gian: O(n) với n là độ dài chuỗi ký tự.
     + Độ phức tạp bộ nhớ: O(n) trong trường hợp xấu nhất (chuỗi chỉ gồm toàn dấu ngoặc mở).

   - Bài 2 (Mô phỏng quầy dịch vụ - Queue):
     + Giải pháp: Dùng Queue<Customer> quản lý danh sách khách hàng đến theo thứ tự FIFO và mảng counterFreeTime lưu thời điểm rảnh tiếp theo của k quầy. Với mỗi khách được lấy ra khỏi Queue (Dequeue), thuật toán duyệt k quầy để chọn quầy rảnh sớm nhất, tính thời điểm bắt đầu = max(thời điểm quầy rảnh, thời điểm khách đến), tính thời gian chờ và cập nhật thời điểm rảnh mới cho quầy đó.
     + Độ phức tạp thời gian: O(n * k) với n là số lượng khách hàng và k là số quầy dịch vụ.
     + Độ phức tạp bộ nhớ: O(n + k) cho hàng chờ Queue và mảng lưu trạng thái k quầy.

   - Bài 3 (Tự cài đặt Hàng đợi vòng - Circular Queue):
     + Giải pháp: Xây dựng lớp CircularQueue<T> sử dụng mảng tĩnh items, quản lý bởi các biến front, count và thuộc tính Capacity. Chỉ số rear tính bằng (front + count) % Capacity. Khi count == Capacity, tự động nhân đôi dung lượng mảng và chép lại dữ liệu theo đúng thứ tự logic tính từ front sang mảng mới rồi gán front = 0 để bảo toàn cơ chế FIFO.
     + Độ phức tạp thời gian: Thao tác Enqueue đạt O(1) khấu hao; các thao tác Dequeue, Peek, Count, Capacity đạt O(1).
     + Độ phức tạp bộ nhớ: O(Capacity) tương ứng với dung lượng mảng lưu trữ.

3. Các ca kiểm thử đã dùng và thời gian chạy ở các ca lớn:
   - Bài 1 (10 ca kiểm thử): Bao gồm các ca thông thường, đóng sai loại, mở chưa đóng, đóng thừa, chuỗi rỗng, ngoặc đan xen, không ngoặc, nhiều ngoặc mở chưa đóng và 2 ca lớn.
     + Thời gian ca lớn: Ca 9 (500,000 ngoặc mở + 500,000 ngoặc đóng) thực thi trong ~12 ms; Ca 10 (1,000,000 ngoặc mở) thực thi trong ~18 ms (đều đáp ứng yêu cầu < 1 giây).
   - Bài 2 (6 ca kiểm thử): Bao gồm các ca 1 quầy, 2 quầy, khách đến cùng lúc, quầy rảnh nghỉ chờ khách, số quầy lớn hơn số khách (k > n) và 1 ca lớn.
     + Thời gian ca lớn: Ca 6 (n = 100,000 khách, k = 10 quầy) thực thi trong ~25 ms (đáp ứng yêu cầu < 1 giây).
   - Bài 3 (7 ca kiểm thử): Bao gồm các ca hàng rỗng ném ngoại lệ, hàng vừa đầy, quay vòng chỉ số không mở rộng, mở rộng khi front = 0, mở rộng khi front != 0, lặp 1,000,000 lần Enqueue/Dequeue, và 1 ca lớn.
     + Thời gian ca lớn: Ca 6 (lặp 1,000,000 lần Enqueue/Dequeue) chạy ổn định không phát sinh lỗi; Ca 7 (100,000 phần tử) thực thi trong ~8 ms với Capacity nâng lên 131,072 (đáp ứng yêu cầu < 1 giây).

     
     Quy định sử dụng AI
     Tôi đã sử dụng AI nhằm hỗ trợ bài làm, giải thích từng chỗ để dễ hiểu hơn. Phần được hỗ trợ gồm các phân tích các độ phức tạp thuật toán, phần comment để hiểu bài, code và phần giải thích. 