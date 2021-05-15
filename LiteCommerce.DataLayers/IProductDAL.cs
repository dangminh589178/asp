using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
  /// <summary>
  /// Định nghĩa các phép xử lý dữ liệu liên quan đến hàng hóa
  /// </summary>
  public interface IProductDAL
  {
    /// <summary>
    /// Lấy danh sách các mặt hàng (Phân trang , tìm kiếm, lọc dữ liệu)
    /// </summary>
    /// <param name="page">trang</param>
    /// <param name="pageSize">kích thước trang</param>
    /// <param name="categoryId">Mã loại hàng(0 nếu không lọc theo loại hàng)</param>
    /// <param name="supplierId">Mã nhà cung cấp (0 nếu không lọc mặt hàng theo nhà cung cấp)</param>
    /// <param name="searchValue">Tên của mặt hàng tìm kiếm(Chuỗi rỗng nếu không muốn tìm kiếm)</param>
    /// <returns></returns>
    List<Product> List(int page, int pageSize, int categoryId, int supplierId, string searchValue);

    /// <summary>
    /// đếm số lượng hàng hóa. hỗ trợ phân trang
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="supplierId"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>
    int Count(int categoryId, int supplierId, string searchValue);

    /// <summary>
    /// Lấy thông tin mặt hàng theo mã
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Product Get(int productId);

    /// <summary>
    /// lấy thông tin mặt hàng và các thông tin liên quan theo mã
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    ProductEx GetEx(int productId);

    /// <summary>
    /// Bổ sung một mặt hàng mới (Hàm trả về mặt hàng bổ sung nếu thành công
    /// Trả về 0 nếu không bổ sung được)
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    int Add(Product data);

    /// <summary>
    /// Cập nhật thông tin của mặt hàng
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    bool Update(Product data);

    /// <summary>
    /// Xóa một mặt hàng theo mã (Khi xóa 1 mặt hàng thì đồng thời cũng xóa các thuộc tính và thư viện ảnh của mặt hàng đó)
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    bool Delete(int productId);

    /// <summary>
    /// Lấy danh sách các thuộc tính của một Product (sắp xếp theo DisplayOrder)
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    List<ProductAttribute> ListAttributes(int productId);

    /// <summary>
    /// Lấy thông tin chi tiết của một thuộc tính 
    /// </summary>
    /// <param name="attributeId"></param>
    /// <returns></returns>
    ProductAttribute GetAttribute(long attributeId);

    /// <summary>
    /// Bổ sung một Attribute cho mặt hàng
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    long AddAttribute(ProductAttribute data);

    /// <summary>
    /// Cập nhật thuộc tính cho mặt hàng
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    bool UpdateAttribute(ProductAttribute data);

    /// <summary>
    /// Xóa thuộc tính
    /// </summary>
    /// <param name="attributeId"></param>
    /// <returns></returns>
    bool DeleteAttribute(long attributeId);

    /// <summary>
    /// Lấy danh sách hình ảnh của một mặt hàng
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    List<ProductGallery> ListGalleries(int productId);

    /// <summary>
    /// Lấy thông tin của một hình ảnh
    /// </summary>
    /// <param name="galleryId"></param>
    /// <returns></returns>
    ProductGallery GetGallery(long galleryId);
    /// <summary>
    /// Bổ sung hình ảnh cho mặt hàng 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    long AddGallery(ProductGallery data);
    /// <summary>
    ///  Cập nhật thông tin ảnh 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    bool UpdateGallery(ProductGallery data);
    /// <summary>
    /// Xóa một ảnh
    /// </summary>
    /// <param name="galleryId"></param>
    /// <returns></returns>
    bool DeleteGallery(long galleryId);
  }
}
