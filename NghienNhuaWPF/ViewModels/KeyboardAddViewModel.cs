using BusinessLogic.Interfaces;
using Microsoft.Win32;
using Models;
using NghienNhuaWPF.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class KeyboardAddViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly IKeyboardServices _keyboardServices;

        public ICommand AddKeyboardCommand { get; }
        public ICommand SelectImagesCommand { get; }

        private List<string> selectedImageFiles = new List<string>();

        public KeyboardAddViewModel(IProductService productService, IKeyboardServices keyboardServices)
        {
            _productService = productService;
            _keyboardServices = keyboardServices;

            SelectImagesCommand = new RelayCommand(SelectImages);
            AddKeyboardCommand = new RelayCommand(AddKeyboard);
        }

        private string _proName;
        public string ProName
        {
            get { return _proName; }
            set
            {
                _proName = value;
                OnPropertyChanged(nameof(ProName));
            }
        }

        private int? _proQuantity;
        public int? ProQuantity
        {
            get { return _proQuantity; }
            set
            {
                _proQuantity = value;
                OnPropertyChanged(nameof(ProQuantity));
            }
        }

        private string _proPrice;
        public string ProPrice
        {
            get { return _proPrice; }
            set
            {
                _proPrice = value;
                OnPropertyChanged(nameof(ProPrice));
            }
        }

        private string _proImage;
        public string ProImage
        {
            get { return _proImage; }
            set
            {
                _proImage = value;
                OnPropertyChanged(nameof(ProImage));
            }
        }

        private string _proDescription;
        public string ProDescription
        {
            get { return _proDescription; }
            set
            {
                _proDescription = value;
                OnPropertyChanged(nameof(ProDescription));
            }
        }

        private string _proDiscount;
        public string ProDiscount
        {
            get { return _proDiscount; }
            set
            {
                _proDiscount = value;
                OnPropertyChanged(nameof(ProDiscount));
            }
        }

        private DateTime? _proDate;
        public DateTime? ProDate
        {
            get { return _proDate; }
            set
            {
                _proDate = value;
                OnPropertyChanged(nameof(ProDate));
            }
        }

        private string _proCategory;
        public string ProCategory
        {
            get { return _proCategory; }
            set
            {
                _proCategory = value;
                OnPropertyChanged(nameof(ProCategory));
            }
        }

        private string _proBrand;
        public string ProBrand
        {
            get { return _proBrand; }
            set
            {
                _proBrand = value;
                OnPropertyChanged(nameof(ProBrand));
            }
        }

        private string _proOrigin;
        public string ProOrigin
        {
            get { return _proOrigin; }
            set
            {
                _proOrigin = value;
                OnPropertyChanged(nameof(ProOrigin));
            }
        }

        private string _kbLed;
        public string KbLed
        {
            get { return _kbLed; }
            set
            {
                _kbLed = value;
                OnPropertyChanged(nameof(KbLed));
            }
        }

        private string _kbMode;
        public string KbMode
        {
            get { return _kbMode; }
            set
            {
                _kbMode = value;
                OnPropertyChanged(nameof(KbMode));
            }
        }

        private string _kbSwitch;
        public string KbSwitch
        {
            get { return _kbSwitch; }
            set
            {
                _kbSwitch = value;
                OnPropertyChanged(nameof(KbSwitch));
            }
        }

        private string _kbKeycap;
        public string KbKeycap
        {
            get { return _kbKeycap; }
            set
            {
                _kbKeycap = value;
                OnPropertyChanged(nameof(KbKeycap));
            }
        }

        private string _kbPlate;
        public string KbPlate
        {
            get { return _kbPlate; }
            set
            {
                _kbPlate = value;
                OnPropertyChanged(nameof(KbPlate));
            }
        }

        private string _kbCase;
        public string KbCase
        {
            get { return _kbCase; }
            set
            {
                _kbCase = value;
                OnPropertyChanged(nameof(KbCase));
            }
        }

        // Lệnh cho phép người dùng chọn ảnh
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
                ProImage = string.Join("&", selectedImageFiles.Select(Path.GetFileName));
            }
        }

        private async void AddKeyboard(object obj)
        {
            // Kiểm tra tính hợp lệ của input
            if (!ValidateInput())
            {
                return;
            }

            try
            {
                // Lưu ảnh vào thư mục Images nếu đã chọn ảnh
                if (selectedImageFiles != null && selectedImageFiles.Count > 0)
                {
                    var savedImages = SaveImagesToFolder(selectedImageFiles);
                    if (savedImages != null && savedImages.Count > 0)
                    {
                        ProImage = string.Join("&", savedImages); // Lưu lại tên ảnh đã được lưu vào cơ sở dữ liệu
                    }
                }
                else
                {
                    MessageBox.Show("Bạn chưa chọn ảnh nào. Vui lòng chọn ít nhất một ảnh.");
                    return;
                }

                // Tạo Product và Keyboard như trước
                var product = new Product
                {
                    ProName = this.ProName,
                    ProQuantity = this.ProQuantity,
                    ProPrice = this.ProPrice,
                    ProImage = this.ProImage, // Đã chứa tên của các ảnh
                    ProDescription = this.ProDescription,
                    ProDiscount = this.ProDiscount,
                    ProDate = DateTime.Now,
                    ProCategory = "KeyBoard",
                    ProBrand = this.ProBrand,
                    ProOrigin = this.ProOrigin,
                };

                await _productService.AddProduct(product);
                int createdProId = product.ProId;

                var keyboard = new KeyBoard
                {
                    KbLed = this.KbLed,
                    KbMode = this.KbMode,
                    KbSwitch = this.KbSwitch,
                    KbKeycap = this.KbKeycap,
                    KbPlate = this.KbPlate,
                    KbCase = this.KbCase,
                    ProId = createdProId,
                };

                await _keyboardServices.Add(keyboard);
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm sản phẩm.");
                throw new Exception("An error occurred while adding the product.", ex);
            }
        }

        // Lưu ảnh vào thư mục Images
        private List<string> SaveImagesToFolder(List<string> imageFiles)
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
                File.Copy(file, destFileName, true);
                savedImages.Add(Path.GetFileName(file));
            }

            return savedImages;
        }
        private void CloseWindow()
        {
            // Tìm cửa sổ cha của ViewModel và đóng nó
            var window = Application.Current.Windows.OfType<Window>()
                         .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        private bool ValidateInput()
        {
            // Kiểm tra tính hợp lệ của các thuộc tính
            if (string.IsNullOrWhiteSpace(ProName) ||
                string.IsNullOrWhiteSpace(ProImage) ||
                string.IsNullOrWhiteSpace(ProPrice) ||
                string.IsNullOrWhiteSpace(ProDescription) ||
                string.IsNullOrWhiteSpace(ProBrand) ||
                string.IsNullOrWhiteSpace(ProOrigin) ||
                string.IsNullOrWhiteSpace(KbLed) ||
                string.IsNullOrWhiteSpace(KbSwitch) ||
                string.IsNullOrWhiteSpace(KbCase) ||
                string.IsNullOrWhiteSpace(KbMode) ||
                string.IsNullOrWhiteSpace(KbPlate) ||
                string.IsNullOrWhiteSpace(KbKeycap))
            {
                MessageBox.Show("Please fill all the fields");
                return false;
            }
            if (int.Parse(ProDiscount) < 0 || int.Parse(ProDiscount) > 100)
            {
                MessageBox.Show("Discount must be greater than zero and less than 100");
                return false;
            }
            if (int.Parse(ProPrice) < 0)
            {
                MessageBox.Show("Price cannot be less than zero");

                return false;
            }
            if (ProQuantity < 0)
            {
                MessageBox.Show("Quantity must not be less than zero");

                return false;
            }
            return true;
        }

    }
}
