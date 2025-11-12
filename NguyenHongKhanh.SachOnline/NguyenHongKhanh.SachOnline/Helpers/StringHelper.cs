using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NguyenHongKhanh.SachOnline.Helpers
{
    /// <summary>
    /// Helper class để xử lý chuỗi, đặc biệt là tạo MetaTitle không dấu
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Loại bỏ dấu tiếng Việt và chuyển thành chữ không dấu
        /// </summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi không dấu</returns>
        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            // Normalize string
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            // Xử lý các ký tự đặc biệt của tiếng Việt
            string result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            // Thay thế các ký tự đặc biệt
            result = result.Replace("Đ", "D").Replace("đ", "d");

            return result;
        }

        /// <summary>
        /// Tạo URL thân thiện từ tiêu đề
        /// </summary>
        /// <param name="text">Tiêu đề</param>
        /// <returns>URL-friendly string</returns>
        public static string GenerateSlug(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            // Loại bỏ dấu
            string slug = text.RemoveDiacritics();

            // Chuyển về chữ thường
            slug = slug.ToLower();

            // Loại bỏ các ký tự không hợp lệ
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Thay thế nhiều khoảng trắng liên tiếp bằng 1 khoảng trắng
            slug = Regex.Replace(slug, @"\s+", " ").Trim();

            // Thay thế khoảng trắng bằng dấu gạch ngang
            slug = Regex.Replace(slug, @"\s", "-");

            // Loại bỏ các dấu gạch ngang liên tiếp
            slug = Regex.Replace(slug, @"-+", "-");

            return slug;
        }

        /// <summary>
        /// Cắt chuỗi theo độ dài và thêm dấu ...
        /// </summary>
        /// <param name="text">Chuỗi cần cắt</param>
        /// <param name="maxLength">Độ dài tối đa</param>
        /// <returns>Chuỗi đã cắt</returns>
        public static string Truncate(this string text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            if (text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength) + "...";
        }

        /// <summary>
        /// Loại bỏ HTML tags
        /// </summary>
        /// <param name="html">Chuỗi HTML</param>
        /// <returns>Plain text</returns>
        public static string StripHtml(this string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return html;

            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }
    }
}
