USE [FUNewsManagement];
GO

-- 1. Cập nhật bảng SystemAccount sang Tiếng Việt và đuôi @gmail.com
UPDATE [dbo].[SystemAccount]
SET [AccountName] = N'Nguyễn Văn An',
    [AccountEmail] = N'nguyenvanan@gmail.com',
    [AccountRole] = 2,
    [AccountPassword] = N'@1'
WHERE [AccountID] = 1;

UPDATE [dbo].[SystemAccount]
SET [AccountName] = N'Trần Minh Anh',
    [AccountEmail] = N'tranminhanh@gmail.com',
    [AccountRole] = 2,
    [AccountPassword] = N'@1'
WHERE [AccountID] = 2;

UPDATE [dbo].[SystemAccount]
SET [AccountName] = N'Lê Văn Bình',
    [AccountEmail] = N'levanbinh@gmail.com',
    [AccountRole] = 1,
    [AccountPassword] = N'@1'
WHERE [AccountID] = 3;

UPDATE [dbo].[SystemAccount]
SET [AccountName] = N'Phạm Thị Hương',
    [AccountEmail] = N'phamthihuong@gmail.com',
    [AccountRole] = 1,
    [AccountPassword] = N'@1'
WHERE [AccountID] = 4;

UPDATE [dbo].[SystemAccount]
SET [AccountName] = N'Hoàng Quốc Việt',
    [AccountEmail] = N'hoangquocviet@gmail.com',
    [AccountRole] = 1,
    [AccountPassword] = N'@1'
WHERE [AccountID] = 5;
GO

-- 2. Cập nhật bảng Category sang Tiếng Việt
UPDATE [dbo].[Category]
SET [CategoryName] = N'Tin tức Học thuật',
    [CategoryDesciption] = N'Chuyên mục cập nhật các công trình nghiên cứu khoa học, bổ nhiệm giảng viên và các thông báo học thuật quan trọng.'
WHERE [CategoryID] = 1;

UPDATE [dbo].[Category]
SET [CategoryName] = N'Công tác Sinh viên',
    [CategoryDesciption] = N'Chuyên mục tổng hợp các hoạt động phong trào, sự kiện, câu lạc bộ và thể thao dành cho sinh viên toàn trường.'
WHERE [CategoryID] = 2;

UPDATE [dbo].[Category]
SET [CategoryName] = N'An ninh Học đường',
    [CategoryDesciption] = N'Chuyên mục thông tin về các biện pháp đảm bảo an toàn, y tế và tiện ích khuôn viên cho sinh viên và giảng viên.'
WHERE [CategoryID] = 3;

UPDATE [dbo].[Category]
SET [CategoryName] = N'Tin tức Cựu sinh viên',
    [CategoryDesciption] = N'Chuyên mục vinh danh thành tựu, sự nghiệp và các chương trình kết nối cộng đồng cựu sinh viên Đại học FPT.'
WHERE [CategoryID] = 4;

UPDATE [dbo].[Category]
SET [CategoryName] = N'Đồ án Tốt nghiệp',
    [CategoryDesciption] = N'Chuyên mục tổng hợp các dự án nghiên cứu và sản phẩm công nghệ xuất sắc từ kỳ bảo vệ đồ án tốt nghiệp.'
WHERE [CategoryID] = 5;
GO

-- 3. Cập nhật bảng Tag sang Tiếng Việt
UPDATE [dbo].[Tag] SET [TagName] = N'Giáo dục', [Note] = N'Tin tức đào tạo và giáo dục đại học' WHERE [TagID] = 1;
UPDATE [dbo].[Tag] SET [TagName] = N'Công nghệ', [Note] = N'Xu hướng công nghệ và chuyển đổi số' WHERE [TagID] = 2;
UPDATE [dbo].[Tag] SET [TagName] = N'Nghiên cứu', [Note] = N'Công trình nghiên cứu khoa học' WHERE [TagID] = 3;
UPDATE [dbo].[Tag] SET [TagName] = N'Đổi mới sáng tạo', [Note] = N'Khởi nghiệp và sáng tạo trẻ' WHERE [TagID] = 4;
UPDATE [dbo].[Tag] SET [TagName] = N'Đời sống sinh viên', [Note] = N'Hoạt động ngoại khóa và câu lạc bộ' WHERE [TagID] = 5;
UPDATE [dbo].[Tag] SET [TagName] = N'Giảng viên', [Note] = N'Thành tựu và hoạt động giảng viên' WHERE [TagID] = 6;
UPDATE [dbo].[Tag] SET [TagName] = N'Cựu sinh viên', [Note] = N'Mạng lưới cựu sinh viên thành đạt' WHERE [TagID] = 7;
UPDATE [dbo].[Tag] SET [TagName] = N'Sự kiện', [Note] = N'Hội thảo và sự kiện nổi bật tại trường' WHERE [TagID] = 8;
UPDATE [dbo].[Tag] SET [TagName] = N'Học liệu', [Note] = N'Tài nguyên học tập và học bổng' WHERE [TagID] = 9;
GO

-- 4. Cập nhật bảng NewsArticle sang Tiếng Việt (và gán CreatedByID = 3, 4 cho đúng Staff)
UPDATE [dbo].[NewsArticle]
SET [NewsTitle] = N'Đại học FPT vinh danh các cựu sinh viên xuất sắc trên nhiều lĩnh vực',
    [Headline] = N'Hành trình tỏa sáng của mạng lưới cựu sinh viên FPT trên thị trường công nghệ toàn cầu',
    [NewsContent] = N'Trường Đại học FPT vừa tổ chức lễ vinh danh các cựu sinh viên tiêu biểu đã đạt được những thành tựu nổi bật trong nhiều lĩnh vực khác nhau, khẳng định chất lượng đào tạo gắn liền với thực tiễn của nhà trường.

Những câu chuyện thành công đa dạng: Từ những nhà sáng lập startup công nghệ đến các chuyên gia quản lý cấp cao tại các tập đoàn đa quốc gia, cựu sinh viên nhà trường đã tạo nên dấu ấn mạnh mẽ trong ngành công nghiệp phần mềm.

Cơ hội kết nối rộng mở: Mạng lưới cựu sinh viên vững mạnh đóng vai trò cầu nối quan trọng trong việc thúc đẩy hợp tác kinh doanh và chia sẻ cơ hội nghề nghiệp giữa các thế hệ sinh viên.

Đóng góp cho thế hệ kế cận: Nhiều cựu sinh viên đã quay trở lại trường tham gia giảng dạy khách mời, tài trợ quỹ học bổng và cố vấn trực tiếp cho các dự án khởi nghiệp của sinh viên.',
    [NewsSource] = N'Ban Truyền thông FU',
    [CreatedByID] = 3,
    [UpdatedByID] = 3
WHERE [NewsArticleID] = N'1';

UPDATE [dbo].[NewsArticle]
SET [NewsTitle] = N'Hội Cựu sinh viên ra mắt chương trình Cố vấn nghề nghiệp cho tân cử nhân',
    [Headline] = N'Đồng hành cùng sinh viên mới tốt nghiệp chinh phục thị trường lao động chất lượng cao',
    [NewsContent] = N'Hội Cựu sinh viên Đại học FPT chính thức khởi động chương trình Cố vấn nghề nghiệp (Mentorship Program) nhằm hỗ trợ và định hướng cho các tân cử nhân trong giai đoạn chuyển tiếp từ giảng đường sang môi trường doanh nghiệp.

Chương trình kết nối trực tiếp từng sinh viên mới ra trường với các anh chị cựu sinh viên giàu kinh nghiệm dựa trên định hướng chuyên môn và mục tiêu phát triển sự nghiệp cá nhân.

Bên cạnh các buổi cố vấn 1-1, chương trình còn tổ chức chuỗi hội thảo chuyên đề về xây dựng hồ sơ năng lực (CV), kỹ năng phỏng vấn chuyên sâu và nghệ thuật xây dựng mạng lưới quan hệ công việc.

Sự kiện ra mắt chương trình khẳng định cam kết lâu dài của cộng đồng cựu sinh viên trong việc tiếp sức cho thế hệ trẻ tự tin hội nhập và bứt phá.',
    [NewsSource] = N'Hội Cựu sinh viên FU',
    [CreatedByID] = 3,
    [UpdatedByID] = 3
WHERE [NewsArticleID] = N'2';

UPDATE [dbo].[NewsArticle]
SET [NewsTitle] = N'Khoa Kỹ thuật Phần mềm công bố chương trình đào tạo tích hợp AI thế hệ mới',
    [Headline] = N'Đổi mới toàn diện giáo trình và phòng thí nghiệm thực hành theo chuẩn doanh nghiệp quốc tế',
    [NewsContent] = N'Khoa Kỹ thuật Phần mềm tại Đại học FPT vừa chính thức công bố loạt sáng kiến cải tiến chương trình đào tạo nhằm nâng cao trải nghiệm học tập và thúc đẩy năng lực nghiên cứu phát triển phần mềm hiện đại.

Nhà trường đã đưa vào vận hành hệ thống phòng Lab chuyên sâu phục vụ nghiên cứu Kiến trúc đám mây, Phát triển ứng dụng doanh nghiệp (.NET Core) và An toàn thông tin.

Đội ngũ giảng viên đầu ngành: Nhiều giảng viên giàu kinh nghiệm thực chiến đã được bổ nhiệm dẫn dắt các nhóm nghiên cứu trọng điểm, đồng hành sát sao cùng sinh viên trong suốt quá trình làm dự án thực tế.

Chương trình học mới giúp sinh viên làm chủ công nghệ ngay từ năm thứ hai, đáp ứng trọn vẹn nhu cầu nhân lực chất lượng cao của thị trường toàn cầu.',
    [NewsSource] = N'Khoa Kỹ thuật Phần mềm',
    [CreatedByID] = 4,
    [UpdatedByID] = 4
WHERE [NewsArticleID] = N'3';

UPDATE [dbo].[NewsArticle]
SET [NewsTitle] = N'Bổ nhiệm Chuyên gia hàng đầu về Học máy làm Trưởng bộ môn Trí tuệ Nhân tạo',
    [Headline] = N'Bước tiến chiến lược nhằm đẩy mạnh nghiên cứu chuyên sâu về AI và Khoa học dữ liệu tại FU',
    [NewsContent] = N'Trường Đại học FPT tự hào công bố quyết định bổ nhiệm Tiến sĩ đầu ngành trong lĩnh vực Học máy (Machine Learning) vào vị trí Trưởng bộ môn Trí tuệ Nhân tạo (AI), đánh dấu bước phát triển mới trong chiến lược học thuật của nhà trường.

Tân Trưởng bộ môn sở hữu bề dày công bố quốc tế về thuật toán Học sâu (Deep Learning) cũng như kinh nghiệm triển khai các giải pháp AI trong y tế thông minh, tài chính định lượng và tự động hóa.

Trong bài phát biểu nhận nhiệm vụ, thầy chia sẻ tầm nhìn xây dựng các nhóm nghiên cứu sinh viên xuất sắc, phát triển các mô hình ngôn ngữ lớn tiếng Việt và mở rộng hợp tác nghiên cứu với các viện công nghệ quốc tế.',
    [NewsSource] = N'Phòng Tổ chức & Đào tạo',
    [CreatedByID] = 4,
    [UpdatedByID] = 4
WHERE [NewsArticleID] = N'4';

UPDATE [dbo].[NewsArticle]
SET [NewsTitle] = N'Công bố kết quả nghiên cứu đột phá của sinh viên và giảng viên trong lĩnh vực STEM',
    [Headline] = N'Nhóm nghiên cứu liên ngành của Đại học FPT đạt giải thưởng xuất sắc tại hội nghị khoa học quốc tế',
    [NewsContent] = N'Công trình nghiên cứu mới nhất được thực hiện bởi Viện Nghiên cứu và Phát triển Công nghệ Đại học FPT đã mang lại những phát hiện quan trọng trong lĩnh vực STEM, mở ra hướng tiếp cận mới trong việc tối ưu hóa hệ thống nhúng và IoT.

Thành công của dự án là kết quả của sự phối hợp chặt chẽ giữa các giảng viên hướng dẫn và nhóm sinh viên tài năng đến từ nhiều chuyên ngành khác nhau như Công nghệ thông tin, Điện tử viễn thông và Toán ứng dụng.

Kết quả nghiên cứu không chỉ được đăng tải trên tạp chí khoa học uy tín mà còn sẵn sàng chuyển giao ứng dụng vào thực tiễn sản xuất thông minh tại Việt Nam.',
    [NewsSource] = N'Viện Nghiên cứu FU',
    [CreatedByID] = 5,
    [UpdatedByID] = 5
WHERE [NewsArticleID] = N'5';
GO
