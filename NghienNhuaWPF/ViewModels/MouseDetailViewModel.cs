using BusinessLogic.Interfaces;
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
    public class MouseDetailViewModel : BaseViewModel
    {
        public Models.Mouse SelectedMouse { get; set; }

        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly IMouseServices _mouseServices;
        public ICommand SelectImagesCommand { get; }

        private List<string> selectedImageFiles = new List<string>();

        public MouseDetailViewModel(Models.Mouse mouse, IMouseServices mouseServices)
        {
            SelectedMouse = mouse ?? new Models.Mouse();
            _mouseServices = mouseServices;

            UpdateCommand = new RelayCommand(ExecuteUpdateCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
            CancelCommand = new RelayCommand(Cancle);
            SelectImagesCommand = new RelayCommand(SelectImages);
        }
        private void SelectImages(object obj)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                selectedImageFiles = openFileDialog.FileNames.ToList();
                SelectedMouse.Pro.ProImage = string.Join("&", selectedImageFiles.Select(Path.GetFileName));
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

                if (File.Exists(destFileName))
                {
                    var result = MessageBox.Show($"Ảnh '{Path.GetFileName(file)}' đã tồn tại. Bạn có muốn ghi đè không?",
                                                   "Thông báo",
                                                   MessageBoxButton.YesNo,
                                                   MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No)
                    {
                        imagesProcessed = false;
                        continue;
                    }
                }
                if (imagesProcessed)
                {
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
                bool imagesProcessed = true; 

                if (selectedImageFiles != null && selectedImageFiles.Count > 0)
                {
                    var savedImages = SaveImagesToFolder(selectedImageFiles, ref imagesProcessed);
                    if (savedImages != null && savedImages.Count > 0)
                    {
                        SelectedMouse.Pro.ProImage = string.Join("&", savedImages); 
                    }
                }

                if (imagesProcessed)
                {
                    await _mouseServices.Update(SelectedMouse);
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
            if (SelectedMouse != null)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SelectedMouse.Pro.ProQuantity = 0;
                    await _mouseServices.Update(SelectedMouse);
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
            var window = Application.Current.Windows.OfType<Window>()
                         .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        private void Cancle(object obj)
        {
            var window = Application.Current.Windows.OfType<Window>()
                         .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(SelectedMouse.Pro.ProName) ||
                string.IsNullOrWhiteSpace(SelectedMouse.Pro.ProImage) ||
                string.IsNullOrWhiteSpace(SelectedMouse.Pro.ProPrice) ||
                string.IsNullOrWhiteSpace(SelectedMouse.Pro.ProDescription) ||
                string.IsNullOrWhiteSpace(SelectedMouse.Pro.ProBrand) ||
                string.IsNullOrWhiteSpace(SelectedMouse.Pro.ProOrigin) ||
                string.IsNullOrWhiteSpace(SelectedMouse.MouseDpi) ||
                string.IsNullOrWhiteSpace(SelectedMouse.MouseWireLength) ||
                string.IsNullOrWhiteSpace(SelectedMouse.MouseLed) ||
                string.IsNullOrWhiteSpace(SelectedMouse.MouseTypeBattery) ||
                string.IsNullOrWhiteSpace(SelectedMouse.MouseWeight) ||
                string.IsNullOrWhiteSpace(SelectedMouse.MouseCompatibility))
            {
                MessageBox.Show("Please fill all the fields");

                return false;
            }
            if (int.Parse(SelectedMouse.Pro.ProDiscount) < 0 || int.Parse(SelectedMouse.Pro.ProDiscount) > 100)
            {
                MessageBox.Show("Discount must be greater than zero and less than 100");
                return false;
            }
            if (int.Parse(SelectedMouse.Pro.ProPrice) < 0)
            {
                MessageBox.Show("Price cannot be less than zero");
                return false;
            }
            if (SelectedMouse.Pro.ProQuantity < 0)
            {
                MessageBox.Show("Quantity must not be less than zero");
                return false;
            }
            return true;
        }
    }
}
