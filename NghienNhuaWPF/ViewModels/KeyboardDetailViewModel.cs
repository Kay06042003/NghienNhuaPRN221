using BusinessLogic.Interfaces;
using Models;
using NghienNhuaWPF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class KeyboardDetailViewModel : BaseViewModel
    {
        public KeyBoard SelectedKeyboard { get; set; }

        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get;}
        public ICommand CancelCommand { get;  }

        private readonly IKeyboardServices _keyboardServices;

        public KeyboardDetailViewModel(KeyBoard keyboard, IKeyboardServices keyboardServices)
        {
            SelectedKeyboard = keyboard ?? new KeyBoard();
            _keyboardServices = keyboardServices;

            UpdateCommand = new RelayCommand(ExecuteUpdateCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand);

        }
        private async void ExecuteUpdateCommand(object parameter)
        {
            if (ValidateInput())
            {
                await _keyboardServices.Update(SelectedKeyboard);
                MessageBox.Show("Cập nhật thành công!");
            }
            else
            {
                MessageBox.Show("Vui lòng kiểm tra lại các thông tin đã nhập.");
            }
        }

        private async void ExecuteDeleteCommand(object parameter)
        {
            if (SelectedKeyboard != null)
            {
                // Hiển thị hộp thoại xác nhận
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                // Nếu người dùng chọn "Yes", thực hiện xóa
                if (result == MessageBoxResult.Yes)
                {
                    SelectedKeyboard.Pro.ProQuantity = 0; // Đặt số lượng thành 0
                    await _keyboardServices.Update(SelectedKeyboard); // Sử dụng hàm update để lưu thay đổi
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

        private bool ValidateInput()
        {
            // Kiểm tra tính hợp lệ của các thuộc tính
            if (string.IsNullOrWhiteSpace(SelectedKeyboard.Pro.ProName)||
                string.IsNullOrWhiteSpace(SelectedKeyboard.Pro.ProImage) ||
                string.IsNullOrWhiteSpace(SelectedKeyboard.Pro.ProPrice) ||
                string.IsNullOrWhiteSpace(SelectedKeyboard.Pro.ProDescription) ||
                string.IsNullOrWhiteSpace(SelectedKeyboard.Pro.ProCategory) ||
                string.IsNullOrWhiteSpace(SelectedKeyboard.Pro.ProBrand) ||
                string.IsNullOrWhiteSpace(SelectedKeyboard.Pro.ProOrigin) ||
                string.IsNullOrWhiteSpace(SelectedKeyboard.KbLed) ||
                string.IsNullOrWhiteSpace(SelectedKeyboard.KbSwitch) ||
                string.IsNullOrWhiteSpace(SelectedKeyboard.KbCase) ||
                string.IsNullOrWhiteSpace(SelectedKeyboard.KbMode) ||
                string.IsNullOrWhiteSpace(SelectedKeyboard.KbPlate) ||
                string.IsNullOrWhiteSpace(SelectedKeyboard.KbKeycap))
            {
                return false; 
            }
            if(int.Parse(SelectedKeyboard.Pro.ProDiscount) < 0 || int.Parse(SelectedKeyboard.Pro.ProDiscount) > 100) 
            {  
                return false; 
            }
            if(int.Parse(SelectedKeyboard.Pro.ProPrice) < 0)
            {  
                return false; 
            }
            if (SelectedKeyboard.Pro.ProQuantity < 0)
            {
                return false;
            }
            return true;
        }

    }
}
