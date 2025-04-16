# Student Management Web API (.NET 8)

## Giới thiệu

Đây là một ứng dụng Web API được xây dựng bằng .NET 8, tuân thủ các nguyên tắc RESTful API để quản lý thông tin sinh viên và lớp học. Dự án này tập trung vào việc thực hiện các chức năng CRUD (Create, Read, Update, Delete) cơ bản, quản lý quan hệ giữa sinh viên và lớp học, đồng thời áp dụng các kỹ thuật như phân tách lớp Service, xử lý lỗi bằng try-catch và sử dụng Collection List để lưu trữ dữ liệu trong bộ nhớ (in-memory).

## Mục lục

- [Tính năng](#tính-năng)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Cấu trúc dự án](#cấu-trúc-dự-án)
- [Thiết lập và Chạy dự án](#thiết-lập-và-chạy-dự-án)
- [Mô hình dữ liệu](#mô-hình-dữ-liệu)
- [API Endpoints](#api-endpoints)
  - [Quản lý Sinh viên](#quản-lý-sinh-viên)
  - [Quản lý Lớp học](#quản-lý-lớp-học)
  - [Quản lý Quan hệ Sinh viên - Lớp học](#quản-lý-quan-hệ-sinh-viên---lớp-học)
- [Xử lý lỗi](#xử-lý-lỗi)
- [Lưu trữ dữ liệu](#lưu-trữ-dữ-liệu)
- [Validation](#validation)

## Tính năng

Dự án cung cấp các chức năng chính sau:

* **Quản lý Sinh viên:**
    * Thêm mới một sinh viên.
    * Cập nhật thông tin sinh viên theo ID.
    * Lấy thông tin chi tiết sinh viên theo ID.
    * Lấy danh sách tất cả sinh viên.
    * Xóa sinh viên theo ID.
* **Quản lý Lớp học:**
    * Thêm mới một lớp học.
    * Cập nhật thông tin lớp học theo ID.
    * Lấy thông tin chi tiết lớp học theo ID.
    * Lấy danh sách tất cả lớp học.
    * Xóa lớp học theo ID (đồng thời xóa các bản ghi liên kết sinh viên với lớp học đó).
* **Quản lý Quan hệ Sinh viên - Lớp học:**
    * Thêm nhiều sinh viên vào một lớp học cùng lúc.
    * Lấy danh sách sinh viên thuộc một lớp học cụ thể theo ID lớp học.

## Công nghệ sử dụng

* [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* ASP.NET Core Web API
* C#

## Cấu trúc dự án

Dự án được tổ chức theo kiến trúc phân lớp cơ bản:

* **Controllers:** Tiếp nhận HTTP request, gọi đến các Service tương ứng và trả về HTTP response.
* **Services:** Chứa logic nghiệp vụ (business logic), xử lý dữ liệu và tương tác với lớp lưu trữ dữ liệu.
* **DTOs (Data Transfer Objects):** Định nghĩa cấu trúc dữ liệu cho việc trao đổi giữa client và server (request/response bodies), bao gồm cả các quy tắc validation.
* **Models:** Định nghĩa các đối tượng thực thể cốt lõi (Student, Class).
* **Data:** Chứa lớp quản lý dữ liệu trong bộ nhớ (ví dụ: `InMemoryDataStore`).

## Thiết lập và Chạy dự án

1.  **Clone repository:**
    ```bash
    git clone <URL_repository_cua_ban>
    cd <ten_thu_muc_du_an>
    ```
2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```
3.  **Build dự án:**
    ```bash
    dotnet build
    ```
4.  **Chạy dự án:**
    ```bash
    dotnet run
    ```
    API sẽ khởi chạy và lắng nghe tại địa chỉ được cấu hình (thường là `http://localhost:5xxx` và `https://localhost:7xxx`). Bạn có thể xem chi tiết địa chỉ trong output của console.

## Mô hình dữ liệu

* **Student:**
    * `Id` (int/Guid): Mã định danh duy nhất.
    * `StudentName` (string): Tên sinh viên (tối đa 50 ký tự).
    * `StudentCode` (string): Mã số sinh viên.
    * `DateOfBirth` (DateOnly/DateTime): Ngày tháng năm sinh (chỉ quan tâm ngày).
* **Class:**
    * `Id` (int/Guid): Mã định danh duy nhất.
    * `ClassName` (string): Tên lớp (tối đa 50 ký tự).
    * `ClassCode` (string): Mã lớp (tối thiểu 5 ký tự).
    * `MaxStudents` (int): Số sinh viên tối đa (lớn hơn 0).
* **StudentClassRelation** (Hoặc quản lý trực tiếp trong Class/Student model nếu dùng List lồng nhau):
    * `Id` (int/Guid): Mã định danh quan hệ.
    * `StudentId` (int/Guid): Khóa ngoại tới Student.
    * `ClassId` (int/Guid): Khóa ngoại tới Class.

## API Endpoints

Dưới đây là danh sách các endpoints được cung cấp bởi API:

*(Lưu ý: Thay thế `{id}`, `{classId}`, `{studentId}` bằng giá trị ID thực tế)*

### Quản lý Sinh viên

| Method | URL                 | Mô tả                          | Request Body            | Response (Success)      | Response (Error)         |
| :----- | :------------------ | :----------------------------- | :---------------------- | :---------------------- | :----------------------- |
| `POST` | `/api/students`     | Thêm sinh viên mới             | `StudentCreateDto`      | `201 Created` + Student | `400 Bad Request`        |
| `PUT`  | `/api/students/{id}`| Cập nhật sinh viên theo ID     | `StudentUpdateDto`      | `200 OK` / `204 No Content` | `400 Bad Request`, `404 Not Found` |
| `GET`  | `/api/students/{id}`| Lấy chi tiết sinh viên theo ID |                         | `200 OK` + Student      | `404 Not Found`          |
| `GET`  | `/api/students`     | Lấy danh sách sinh viên        |                         | `200 OK` + List<Student>|                          |
| `DELETE`| `/api/students/{id}`| Xóa sinh viên theo ID          |                         | `204 No Content`        | `404 Not Found`          |

### Quản lý Lớp học

| Method | URL               | Mô tả                        | Request Body          | Response (Success)    | Response (Error)         |
| :----- | :---------------- | :--------------------------- | :-------------------- | :-------------------- | :----------------------- |
| `POST` | `/api/classes`    | Thêm lớp học mới             | `ClassCreateDto`      | `201 Created` + Class | `400 Bad Request`        |
| `PUT`  | `/api/classes/{id}`| Cập nhật lớp học theo ID     | `ClassUpdateDto`      | `200 OK` / `204 No Content` | `400 Bad Request`, `404 Not Found` |
| `GET`  | `/api/classes/{id}`| Lấy chi tiết lớp học theo ID |                       | `200 OK` + Class      | `404 Not Found`          |
| `GET`  | `/api/classes`    | Lấy danh sách lớp học        |                       | `200 OK` + List<Class>|                          |
| `DELETE`| `/api/classes/{id}`| Xóa lớp học theo ID (kèm SV)|                       | `204 No Content`      | `404 Not Found`          |

### Quản lý Quan hệ Sinh viên - Lớp học

| Method | URL                               | Mô tả                                     | Request Body         | Response (Success) | Response (Error)                 |
| :----- | :-------------------------------- | :---------------------------------------- | :------------------- | :----------------- | :------------------------------- |
| `POST` | `/api/classes/{classId}/students` | Thêm nhiều sinh viên vào lớp theo ID lớp | `List<StudentIdDto>` | `200 OK` / `204 No Content`  | `400 Bad Request`, `404 Not Found` (Class or Student) |
| `GET`  | `/api/classes/{classId}/students` | Lấy danh sách sinh viên trong lớp theo ID |                      | `200 OK` + List<Student> | `404 Not Found` (Class)          |

*(Ví dụ cấu trúc DTOs - bạn cần điều chỉnh cho phù hợp với code thực tế)*

* **StudentCreateDto:** `{ "studentName": "...", "studentCode": "...", "dateOfBirth": "YYYY-MM-DD" }`
* **StudentUpdateDto:** `{ "studentName": "...", "dateOfBirth": "YYYY-MM-DD" }` *(Có thể chỉ cập nhật các trường cho phép)*
* **ClassCreateDto:** `{ "className": "...", "classCode": "...", "maxStudents": ... }`
* **ClassUpdateDto:** `{ "className": "...", "maxStudents": ... }` *(Có thể chỉ cập nhật các trường cho phép)*
* **List<StudentIdDto>:** `[ { "studentId": ... }, { "studentId": ... } ]` hoặc đơn giản là `[ studentId1, studentId2, ... ]` tùy cách bạn implement.

## Xử lý lỗi

* Các action trong Controller sử dụng khối `try-catch` để bắt các ngoại lệ (exceptions) có thể xảy ra trong tầng Service.
* Các lỗi phổ biến như không tìm thấy tài nguyên (Not Found), dữ liệu không hợp lệ (Bad Request) sẽ được trả về với mã trạng thái HTTP tương ứng (404, 400).
* Các lỗi không mong muốn khác có thể trả về mã `500 Internal Server Error`.

## Lưu trữ dữ liệu

* Dự án này sử dụng `System.Collections.Generic.List<T>` để lưu trữ dữ liệu sinh viên, lớp học và quan hệ giữa chúng trong bộ nhớ (in-memory).
* **Lưu ý:** Dữ liệu sẽ bị mất khi ứng dụng khởi động lại. Đây chỉ là giải pháp tạm thời cho mục đích demo hoặc phát triển ban đầu. Để lưu trữ bền vững, cần tích hợp với cơ sở dữ liệu (SQL Server, PostgreSQL, MongoDB,...).

## Validation

* Các ràng buộc dữ liệu (constraints) được định nghĩa trong các lớp DTO (Data Transfer Objects) sử dụng các Data Annotations của .NET như:
    * `[Required]`
    * `[MaxLength(50)]`
    * `[MinLength(5)]`
    * `[Range(1, int.MaxValue)]` (cho `MaxStudents`)
* ASP.NET Core sẽ tự động kiểm tra các validation này khi model binding diễn ra. Nếu dữ liệu không hợp lệ, Controller sẽ trả về lỗi `400 Bad Request` kèm theo chi tiết lỗi.
