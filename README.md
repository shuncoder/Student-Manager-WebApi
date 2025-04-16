# 📚 Student Management Web API (.NET 8)

<div align="center">
    <img src="./image/Screenshot%202025-04-16%20113018.png" alt="Swagger UI Screenshot" width="80%">
    <p><i>Giao diện Swagger UI của Student Management API</i></p>
</div>

## 📋 Giới thiệu

Đây là một ứng dụng Web API được xây dựng bằng .NET 8, tuân thủ các nguyên tắc RESTful API để quản lý thông tin sinh viên và lớp học. Dự án này tập trung vào việc thực hiện các chức năng CRUD (Create, Read, Update, Delete) cơ bản, quản lý quan hệ giữa sinh viên và lớp học, đồng thời áp dụng các kỹ thuật như phân tách lớp Service, xử lý lỗi bằng try-catch và sử dụng Collection List để lưu trữ dữ liệu trong bộ nhớ (in-memory).

## 📑 Mục lục

- [📚 Student Management Web API (.NET 8)](#-student-management-web-api-net-8)
  - [📋 Giới thiệu](#-giới-thiệu)
  - [📑 Mục lục](#-mục-lục)
  - [✨ Tính năng](#-tính-năng)
  - [🛠️ Công nghệ sử dụng](#️-công-nghệ-sử-dụng)
  - [🏗️ Cấu trúc dự án](#️-cấu-trúc-dự-án)
  - [🚀 Thiết lập và Chạy dự án](#-thiết-lập-và-chạy-dự-án)
  - [📊 Mô hình dữ liệu](#-mô-hình-dữ-liệu)
  - [🔌 API Endpoints](#-api-endpoints)
    - [👨‍🎓 Quản lý Sinh viên](#-quản-lý-sinh-viên)
    - [🏫 Quản lý Lớp học](#-quản-lý-lớp-học)
    - [🔗 Quản lý Quan hệ Sinh viên - Lớp học](#-quản-lý-quan-hệ-sinh-viên---lớp-học)
  - [📝 Cấu trúc DTOs](#-cấu-trúc-dtos)
  - [⚠️ Xử lý lỗi](#️-xử-lý-lỗi)
  - [💾 Lưu trữ dữ liệu](#-lưu-trữ-dữ-liệu)
  - [✅ Validation](#-validation)

## ✨ Tính năng

Dự án cung cấp các chức năng chính sau:

<details open>
<summary><b>Quản lý Sinh viên:</b></summary>

* ➕ Thêm mới một sinh viên.
* 🔄 Cập nhật thông tin sinh viên theo ID.
* 🔍 Lấy thông tin chi tiết sinh viên theo ID.
* 📋 Lấy danh sách tất cả sinh viên.
* ❌ Xóa sinh viên theo ID.
</details>

<details open>
<summary><b>Quản lý Lớp học:</b></summary>

* ➕ Thêm mới một lớp học.
* 🔄 Cập nhật thông tin lớp học theo ID.
* 🔍 Lấy thông tin chi tiết lớp học theo ID.
* 📋 Lấy danh sách tất cả lớp học.
* ❌ Xóa lớp học theo ID (đồng thời xóa các bản ghi liên kết sinh viên với lớp học đó).
</details>

<details open>
<summary><b>Quản lý Quan hệ Sinh viên - Lớp học:</b></summary>

* ➕ Thêm nhiều sinh viên vào một lớp học cùng lúc.
* 🔍 Lấy danh sách sinh viên thuộc một lớp học cụ thể theo ID lớp học.
</details>

## 🛠️ Công nghệ sử dụng

<div align="center">
  <table>
    <tr>
      <td align="center">
        <img src="https://upload.wikimedia.org/wikipedia/commons/e/ee/.NET_Core_Logo.svg" width="50px">
        <br><a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0"><b>.NET 8</b></a>
      </td>
      <td align="center">
        <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/e/ee/.NET_Core_Logo.svg/512px-.NET_Core_Logo.svg.png" width="50px">
        <br><b>ASP.NET Core Web API</b>
      </td>
      <td align="center">
        <img src="https://cdn.worldvectorlogo.com/logos/c--4.svg" width="50px">
        <br><b>C#</b>
      </td>
      <td align="center">
        <img src="https://upload.wikimedia.org/wikipedia/commons/a/ab/Swagger-logo.png" width="50px">
        <br><b>Swagger</b>
      </td>
    </tr>
  </table>
</div>

## 🏗️ Cấu trúc dự án

Dự án được tổ chức theo kiến trúc phân lớp cơ bản:

```
StudentManagementAPI/
├── Controllers/             # Tiếp nhận HTTP requests
│   ├── ClassesController.cs
│   └── StudentsController.cs
├── Services/                # Xử lý logic nghiệp vụ
│   ├── Interfaces/
│   │   ├── IClassService.cs
│   │   └── IStudentService.cs
│   └── Implementations/
│       ├── ClassService.cs
│       └── StudentService.cs
├── Models/                  # Các đối tượng thực thể
│   ├── Class.cs
│   ├── Student.cs
│   └── StudentClass.cs
├── DTOs/                    # Data Transfer Objects
│   ├── ClassDtos.cs
│   └── StudentDtos.cs
└── Data/                    # Lớp lưu trữ dữ liệu
    └── InMemoryDataStore.cs
```

- **Controllers:** Tiếp nhận HTTP request, gọi đến các Service tương ứng và trả về HTTP response.
- **Services:** Chứa logic nghiệp vụ (business logic), xử lý dữ liệu và tương tác với lớp lưu trữ dữ liệu.
- **DTOs (Data Transfer Objects):** Định nghĩa cấu trúc dữ liệu cho việc trao đổi giữa client và server (request/response bodies), bao gồm cả các quy tắc validation.
- **Models:** Định nghĩa các đối tượng thực thể cốt lõi (Student, Class, StudentClass).
- **Data:** Chứa lớp quản lý dữ liệu trong bộ nhớ (InMemoryDataStore).

## 🚀 Thiết lập và Chạy dự án

<details open>
<summary><b>Các bước thiết lập:</b></summary>

1. **Clone repository:**
   ```bash
   git clone https://github.com/shuncoder/Student-Manager-WebApi.git
   cd StudentManagementAPI
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build dự án:**
   ```bash
   dotnet build
   ```

4. **Chạy dự án:**
   ```bash
   dotnet run
   ```
</details>

API sẽ khởi chạy và lắng nghe tại địa chỉ được cấu hình (thường là `http://localhost:5xxx` và `https://localhost:7xxx`). Bạn có thể xem chi tiết địa chỉ trong output của console.

> **Lưu ý:** Dự án tích hợp **Swagger UI** để cung cấp tài liệu API trực quan và cho phép người dùng dễ dàng tương tác, thử nghiệm các endpoints trực tiếp từ trình duyệt. Truy cập `/swagger` sau khi khởi động API.

## 📊 Mô hình dữ liệu

<table>
  <tr>
    <th width="33%">Student</th>
    <th width="33%">Class</th>
    <th width="33%">StudentClass</th>
  </tr>
  <tr>
    <td>
      <ul>
        <li><code>Id</code> (Guid): Mã định danh duy nhất.</li>
        <li><code>Name</code> (string): Tên sinh viên (tối đa 50 ký tự).</li>
        <li><code>StudentCode</code> (string): Mã số sinh viên.</li>
        <li><code>DateOfBirth</code> (DateOnly): Ngày tháng năm sinh.</li>
      </ul>
    </td>
    <td>
      <ul>
        <li><code>Id</code> (Guid): Mã định danh duy nhất.</li>
        <li><code>Name</code> (string): Tên lớp (tối đa 50 ký tự).</li>
        <li><code>ClassCode</code> (string): Mã lớp (tối thiểu 5 ký tự).</li>
        <li><code>MaxStudents</code> (int): Số sinh viên tối đa (lớn hơn 0).</li>
      </ul>
    </td>
    <td>
      <ul>
        <li><code>Id</code> (Guid): Mã định danh quan hệ.</li>
        <li><code>StudentId</code> (Guid): Khóa ngoại tới Student.</li>
        <li><code>ClassId</code> (Guid): Khóa ngoại tới Class.</li>
      </ul>
    </td>
  </tr>
</table>

## 🔌 API Endpoints

Dưới đây là danh sách các endpoints được cung cấp bởi API:

> *Lưu ý: Thay thế `{id}`, `{classId}`, `{studentId}` bằng giá trị GUID thực tế*

### 👨‍🎓 Quản lý Sinh viên

| Method | URL                 | Mô tả                          | Request Body            | Response (Success)      | Response (Error)         |
| :----- | :------------------ | :----------------------------- | :---------------------- | :---------------------- | :----------------------- |
| `POST` | `/api/students`     | Thêm sinh viên mới             | `CreateStudentDto`      | `201 Created` + StudentDto | `400 Bad Request`        |
| `PUT`  | `/api/students/{id}`| Cập nhật sinh viên theo ID     | `UpdateStudentDto`      | `200 OK` + StudentDto | `400 Bad Request`, `404 Not Found` |
| `GET`  | `/api/students/{id}`| Lấy chi tiết sinh viên theo ID |                         | `200 OK` + StudentDto      | `404 Not Found`          |
| `GET`  | `/api/students`     | Lấy danh sách sinh viên        |                         | `200 OK` + List<StudentDto>|                          |
| `DELETE`| `/api/students/{id}`| Xóa sinh viên theo ID          |                         | `204 No Content`        | `404 Not Found`          |

### 🏫 Quản lý Lớp học

| Method | URL               | Mô tả                        | Request Body          | Response (Success)    | Response (Error)         |
| :----- | :---------------- | :--------------------------- | :-------------------- | :-------------------- | :----------------------- |
| `POST` | `/api/classes`    | Thêm lớp học mới             | `CreateClassDto`      | `201 Created` + ClassDto | `400 Bad Request`        |
| `PUT`  | `/api/classes/{id}`| Cập nhật lớp học theo ID     | `UpdateClassDto`      | `200 OK` + ClassDto | `400 Bad Request`, `404 Not Found` |
| `GET`  | `/api/classes/{id}`| Lấy chi tiết lớp học theo ID |                       | `200 OK` + ClassDto      | `404 Not Found`          |
| `GET`  | `/api/classes`    | Lấy danh sách lớp học        |                       | `200 OK` + List<ClassDto>|                          |
| `DELETE`| `/api/classes/{id}`| Xóa lớp học theo ID (kèm SV)|                       | `204 No Content`      | `404 Not Found`          |

### 🔗 Quản lý Quan hệ Sinh viên - Lớp học

| Method | URL                               | Mô tả                                     | Request Body              | Response (Success) | Response (Error)                 |
| :----- | :-------------------------------- | :---------------------------------------- | :------------------------ | :----------------- | :------------------------------- |
| `POST` | `/api/classes/add-students`       | Thêm nhiều sinh viên vào lớp              | `AddStudentsToClassDto`   | `200 OK`           | `400 Bad Request`, `404 Not Found` |
| `GET`  | `/api/classes/{id}/students`      | Lấy danh sách sinh viên trong lớp theo ID |                           | `200 OK` + List<StudentDto> | `404 Not Found`                  |

## 📝 Cấu trúc DTOs

<details>
<summary><b>🧑 CreateStudentDto & UpdateStudentDto</b></summary>

```json
{ 
  "name": "Nguyễn Văn A", 
  "studentCode": "SV001", 
  "dateOfBirth": "2003-05-10" 
}
```
</details>

<details>
<summary><b>🏫 CreateClassDto & UpdateClassDto</b></summary>

```json
// CreateClassDto
{ 
  "name": "Lập Trình C#", 
  "classCode": "CSHARP01", 
  "maxStudents": 30 
}

// UpdateClassDto
{ 
  "name": "Lập Trình C# Nâng Cao", 
  "classCode": "CSHARP01", 
  "maxStudents": 40 
}
```
</details>

<details>
<summary><b>🔗 AddStudentsToClassDto</b></summary>

```json
{ 
  "classId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", 
  "studentIds": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "5da85f64-5717-4562-b3fc-2c963f66afa7"
  ] 
}
```
</details>

## ⚠️ Xử lý lỗi

<div style="background-color: #f8f9fa; padding: 15px; border-radius: 5px;">
<ul>
<li>✅ Các action trong Controller sử dụng khối <code>try-catch</code> để bắt các ngoại lệ (exceptions) có thể xảy ra trong tầng Service.</li>
<li>🔍 Các lỗi phổ biến như không tìm thấy tài nguyên (Not Found), dữ liệu không hợp lệ (Bad Request) sẽ được trả về với mã trạng thái HTTP tương ứng (404, 400).</li>
<li>🔄 Các lỗi không mong muốn khác sẽ được log lại và trả về mã <code>500 Internal Server Error</code>.</li>
</ul>
</div>

## 💾 Lưu trữ dữ liệu

<div style="background-color: #f8f9fa; padding: 15px; border-radius: 5px;">
<ul>
<li>📋 Dự án này sử dụng <code>System.Collections.Generic.List&lt;T&gt;</code> để lưu trữ dữ liệu sinh viên, lớp học và quan hệ giữa chúng trong bộ nhớ (in-memory) thông qua class <code>InMemoryDataStore</code>.</li>
<li>🔧 Dữ liệu mẫu được khởi tạo trong constructor của <code>InMemoryDataStore</code> để demo.</li>
<li>⚠️ <strong>Lưu ý:</strong> Dữ liệu sẽ bị mất khi ứng dụng khởi động lại. Đây chỉ là giải pháp tạm thời cho mục đích demo hoặc phát triển ban đầu. Để lưu trữ bền vững, cần tích hợp với cơ sở dữ liệu (SQL Server, PostgreSQL, MongoDB,...).</li>
</ul>
</div>

## ✅ Validation

Các ràng buộc dữ liệu (constraints) được định nghĩa trong các lớp DTO sử dụng Data Annotations của .NET:

<table>
  <tr>
    <th>Annotation</th>
    <th>Mô tả</th>
    <th>Ví dụ</th>
  </tr>
  <tr>
    <td><code>[Required]</code></td>
    <td>Bắt buộc phải có giá trị</td>
    <td><code>[Required(ErrorMessage = "Tên sinh viên là bắt buộc.")]</code></td>
  </tr>
  <tr>
    <td><code>[MaxLength]</code></td>
    <td>Độ dài tối đa</td>
    <td><code>[MaxLength(50, ErrorMessage = "Tên sinh viên không được vượt quá 50 ký tự.")]</code></td>
  </tr>
  <tr>
    <td><code>[MinLength]</code></td>
    <td>Độ dài tối thiểu</td>
    <td><code>[MinLength(5, ErrorMessage = "Mã lớp phải có ít nhất 5 ký tự.")]</code></td>
  </tr>
  <tr>
    <td><code>[Range]</code></td>
    <td>Giới hạn phạm vi giá trị</td>
    <td><code>[Range(1, int.MaxValue, ErrorMessage = "Số sinh viên tối đa phải lớn hơn 0.")]</code></td>
  </tr>
</table>

ASP.NET Core tự động kiểm tra các validation này khi model binding diễn ra. Nếu dữ liệu không hợp lệ, Controller sẽ trả về lỗi `400 Bad Request` kèm theo chi tiết lỗi bằng tiếng Việt.
