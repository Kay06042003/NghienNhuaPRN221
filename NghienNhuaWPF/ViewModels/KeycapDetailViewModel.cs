using BusinessLogic.Interfaces;
using Models;
using NghienNhuaWPF.Utilities;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class KeycapDetailViewModel : BaseViewModel
    {
        public Keycap SelectedKeycap { get; set; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly IKeycapServices _keycapServices;
        public ICommand SelectImagesCommand { get; }

        private List<string> selectedImageFiles = new List<string>();

        public KeycapDetailViewModel(Keycap selectedKeycap, IKeycapServices keycapServices)
        {
            SelectedKeycap = selectedKeycap ?? new Keycap();
            _keycapServices = keycapServices;

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
                selectedImageFiles = openFileDialog.FileNames.ToList();
                SelectedKeycap.Pro.ProImage = string.Join("&", selectedImageFiles.Select(Path.GetFileName));
            }
        }
        private List<string> SaveImagesToFolder(List<string> imageFiles, ref bool imagesProcessed)
        {
            List<string> savedImages = new List<string>();
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string folderPath = Path.Combine(projectRoot, "Images");

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
                        SelectedKeycap.Pro.ProImage = string.Join("&", savedImages);
                    }
                }

                if (imagesProcessed)
                {
                    await _keycapServices.Update(SelectedKeycap);
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
            if (SelectedKeycap != null)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SelectedKeycap.Pro.ProQuantity = 0; 
                    await _keycapServices.Update(SelectedKeycap);
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
            if (string.IsNullOrWhiteSpace(SelectedKeycap.Pro.ProName) ||
                string.IsNullOrWhiteSpace(SelectedKeycap.Pro.ProImage) ||
                string.IsNullOrWhiteSpace(SelectedKeycap.Pro.ProPrice) ||
                string.IsNullOrWhiteSpace(SelectedKeycap.Pro.ProDescription) ||
                string.IsNullOrWhiteSpace(SelectedKeycap.Pro.ProCategory) ||
                string.IsNullOrWhiteSpace(SelectedKeycap.Pro.ProBrand) ||
                string.IsNullOrWhiteSpace(SelectedKeycap.Pro.ProOrigin) ||
                string.IsNullOrWhiteSpace(SelectedKeycap.KcMaterial) ||
                string.IsNullOrWhiteSpace(SelectedKeycap.KcThickness) ||
                string.IsNullOrWhiteSpace(SelectedKeycap.KcReliability) ||
                string.IsNullOrWhiteSpace(SelectedKeycap.KcLayout))
            {
                MessageBox.Show("Please fill all the fields");

                return false;
            }
            if (int.Parse(SelectedKeycap.Pro.ProDiscount) < 0 || int.Parse(SelectedKeycap.Pro.ProDiscount) > 100)
            {
                MessageBox.Show("Discount must be greater than zero and less than 100");
                return false;
            }
            if (int.Parse(SelectedKeycap.Pro.ProPrice) < 0)
            {
                MessageBox.Show("Price cannot be less than zero");
                return false;
            }
            if (SelectedKeycap.Pro.ProQuantity < 0)
            {
                MessageBox.Show("Quantity must not be less than zero");
                return false;
            }
            return true;
        }

    }
}
