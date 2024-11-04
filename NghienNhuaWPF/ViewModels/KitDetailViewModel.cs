using BusinessLogic.Interfaces;
using Models;
using NghienNhuaWPF.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace NghienNhuaWPF.ViewModels
{
    public class KitDetailViewModel : BaseViewModel
    {
        public Kit SelectedKit { get; set; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly IKitServices _kitService;
        public ICommand SelectImagesCommand { get; }

        private List<string> selectedImageFiles = new List<string>();

        public KitDetailViewModel(Kit selectedKit, IKitServices kitService)
        {
            SelectedKit = selectedKit ?? new Kit();
            _kitService = kitService;

            UpdateCommand = new RelayCommand(ExecuteUpdateCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
            CancelCommand = new RelayCommand(Cancle);
            SelectImagesCommand = new RelayCommand(SelectImages);
        }
        private void SelectImages(object obj)
        {
            // Mở hộp thoại chọn file
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                // Lưu các file ảnh đã chọn vào danh sách tạm thời
                selectedImageFiles = openFileDialog.FileNames.ToList();

                // Hiển thị tên ảnh trong TextBox
                SelectedKit.Pro.ProImage = string.Join("&", selectedImageFiles.Select(Path.GetFileName));
            }
        }
        private List<string> SaveImagesToFolder(List<string> imageFiles, ref bool imagesProcessed)
        {
            List<string> savedImages = new List<string>();
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName;
            string folderPath = Path.Combine(projectRoot, "NghienNhuaMVC", "wwwroot", "Images", "Product");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            foreach (string file in imageFiles)
            {
                string destFileName = Path.Combine(folderPath, Path.GetFileName(file));

                // Kiểm tra tệp đã tồn tại
                if (File.Exists(destFileName))
                {
                    // Hiển thị hộp thoại xác nhận
                    var result = MessageBox.Show($"Ảnh '{Path.GetFileName(file)}' đã tồn tại. Bạn có muốn ghi đè không?",
                                                   "Thông báo",
                                                   MessageBoxButton.YesNo,
                                                   MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No)
                    {
                        // Nếu người dùng không muốn ghi đè, đặt biến imagesProcessed thành false
                        imagesProcessed = false;
                        continue; // Tiếp tục với ảnh tiếp theo
                    }
                }
                if (imagesProcessed)
                {   // Sao chép tệp vào thư mục
                    File.Copy(file, destFileName, true);
                    savedImages.Add(Path.GetFileName(file));
                }
            }

            return savedImages;
        }


        private async void ExecuteUpdateCommand(object parameter)
        {
            if (ValidateInput())
            {
                bool imagesProcessed = true; // Biến theo dõi trạng thái xử lý ảnh

                if (selectedImageFiles != null && selectedImageFiles.Count > 0)
                {
                    var savedImages = SaveImagesToFolder(selectedImageFiles, ref imagesProcessed);
                    if (savedImages != null && savedImages.Count > 0)
                    {
                        SelectedKit.Pro.ProImage = string.Join("&", savedImages); // Lưu lại tên ảnh đã được lưu vào cơ sở dữ liệu
                    }
                }
                // Chỉ cập nhật nếu tất cả ảnh đã được xử lý thành công
                if (imagesProcessed)
                {
                    await _kitService.Update(SelectedKit);
                    MessageBox.Show("Cập nhật thành công!");
                }
                else
                {
                    MessageBox.Show("Cập nhật bị hủy do không ghi đè ảnh.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng kiểm tra lại các thông tin đã nhập.");
            }
        }


        private async void ExecuteDeleteCommand(object parameter)
        {
            if (SelectedKit != null)
            {
                // Hiển thị hộp thoại xác nhận
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                // Nếu người dùng chọn "Yes", thực hiện xóa
                if (result == MessageBoxResult.Yes)
                {
                    SelectedKit.Pro.ProQuantity = 0; // Đặt số lượng thành 0
                    await _kitService.Update(SelectedKit); // Sử dụng hàm update để lưu thay đổi
                    MessageBox.Show("Đã xóa thành công!");
                    CloseWindow();
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy bàn phím để xóa.");
            }
        }

        private void CloseWindow()
        {
            // Tìm cửa sổ cha của ViewModel và đóng nó
            var window = Application.Current.Windows.OfType<Window>()
                         .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        private void Cancle(object obj)
        {
            // Tìm cửa sổ cha của ViewModel và đóng nó
            var window = Application.Current.Windows.OfType<Window>()
                         .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        private bool ValidateInput()
        {
            // Kiểm tra tính hợp lệ của các thuộc tính
            if (string.IsNullOrWhiteSpace(SelectedKit.Pro.ProName) ||
                string.IsNullOrWhiteSpace(SelectedKit.Pro.ProImage) ||
                string.IsNullOrWhiteSpace(SelectedKit.Pro.ProDescription) ||
                string.IsNullOrWhiteSpace(SelectedKit.Pro.ProBrand) ||
                string.IsNullOrWhiteSpace(SelectedKit.Pro.ProOrigin) ||
                string.IsNullOrWhiteSpace(SelectedKit.KitLayout) ||
                string.IsNullOrWhiteSpace(SelectedKit.KitCircuit) ||
                string.IsNullOrWhiteSpace(SelectedKit.KitPlate) ||
                string.IsNullOrWhiteSpace(SelectedKit.KitMode) ||
                string.IsNullOrWhiteSpace(SelectedKit.KitCase))
            {
                MessageBox.Show("Please fill all the fields");
                return false;
            }
            if (int.Parse(SelectedKit.Pro.ProDiscount) < 0 || int.Parse(SelectedKit.Pro.ProDiscount) > 100)
            {
                MessageBox.Show("Discount must be greater than zero and less than 100");
                return false;
            }
            if (int.Parse(SelectedKit.Pro.ProPrice) < 0)
            {
                MessageBox.Show("Price cannot be less than zero");
                return false;
            }
            if (SelectedKit.Pro.ProQuantity < 0)
            {
                MessageBox.Show("Quantity must not be less than zero");
                return false;
            }
            return true;
        }
    }
}
