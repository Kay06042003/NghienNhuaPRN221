using BusinessLogic.Interfaces;
using Models;
using NghienNhuaWPF.Utilities;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class KeycapAddViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly IKeycapServices _keycapServices;
        public ICommand AddKeycapCommand { get; }
        public ICommand SelectImagesCommand { get; }
        public ICommand BackToListCommand { get; }

        private List<string> selectedImageFiles = new List<string>();

        public KeycapAddViewModel(IKeycapServices keycapServices, IProductService productService)
        {
            _keycapServices = keycapServices;
            _productService = productService;

            AddKeycapCommand = new RelayCommand(AddKeycap);
            SelectImagesCommand = new RelayCommand(SelectImages);
            BackToListCommand = new RelayCommand(BackToList);
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

        private string _kcMaterial;
        public string KcMaterial
        {
            get { return _kcMaterial; }
            set
            {
                _kcMaterial = value;
                OnPropertyChanged(nameof(KcMaterial));
            }
        }

        private string _kcLayout;
        public string KcLayout
        {
            get { return _kcLayout; }
            set
            {
                _kcLayout = value;
                OnPropertyChanged(nameof(KcLayout));
            }
        }

        private string _kcThickness;
        public string KcThickness
        {
            get { return _kcThickness; }
            set
            {
                _kcThickness = value;
                OnPropertyChanged(nameof(KcThickness));
            }
        }

        private string _kcReliability;
        public string KcReliability
        {
            get { return _kcReliability; }
            set
            {
                _kcReliability = value;
                OnPropertyChanged(nameof(KcReliability));
            }
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
                ProImage = string.Join("&", selectedImageFiles.Select(Path.GetFileName));
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

        private async void AddKeycap(object obj)
        {
            if (!ValidateInput())
            {
                return;
            }
            bool imagesProcessed = true;
            try
            {
                if (selectedImageFiles != null && selectedImageFiles.Count > 0)
                {
                    var savedImages = SaveImagesToFolder(selectedImageFiles, ref imagesProcessed);
                    if (savedImages != null && savedImages.Count > 0)
                    {
                        ProImage = string.Join("&", savedImages);
                    }
                }
                else
                {
                    MessageBox.Show("Bạn chưa chọn ảnh nào. Vui lòng chọn ít nhất một ảnh.");
                    return;
                }
                if (!imagesProcessed)
                {
                    MessageBox.Show("Thêm bị hủy do không ghi đè ảnh.");
                    return;
                }

                var product = new Product
                {
                    ProName = this.ProName,
                    ProQuantity = this.ProQuantity,
                    ProPrice = this.ProPrice,
                    ProImage = this.ProImage,
                    ProDescription = this.ProDescription,
                    ProDiscount = this.ProDiscount,
                    ProDate = DateTime.Now,
                    ProCategory = "Keycap",
                    ProBrand = this.ProBrand,
                    ProOrigin = this.ProOrigin,
                };

                await _productService.AddProduct(product);
                int createdProId = product.ProId;

                var keycap = new Keycap
                {
                    KcMaterial = this.KcMaterial,
                    KcLayout = this.KcLayout,
                    KcReliability = this.KcReliability,
                    KcThickness = this.KcThickness,
                    ProId = createdProId,
                };

                await _keycapServices.Add(keycap);
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm sản phẩm.");
                throw new Exception("An error occurred while adding the product.", ex);
            }
        }
        private void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<Window>()
                         .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        private void BackToList(object obj)
        {
            var window = Application.Current.Windows.OfType<Window>()
                         .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(ProName) ||
                string.IsNullOrWhiteSpace(ProImage) ||
                string.IsNullOrWhiteSpace(ProDescription) ||
                string.IsNullOrWhiteSpace(ProBrand) ||
                string.IsNullOrWhiteSpace(ProOrigin) ||
                string.IsNullOrWhiteSpace(KcMaterial) ||
                string.IsNullOrWhiteSpace(KcThickness) ||
                string.IsNullOrWhiteSpace(KcReliability) ||
                string.IsNullOrWhiteSpace(KcLayout))
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
