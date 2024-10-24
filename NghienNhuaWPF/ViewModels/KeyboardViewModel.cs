using BusinessLogic.Interfaces;
using Models;
using NghienNhuaWPF.Utilities;
using NghienNhuaWPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class KeyboardViewModel : BaseViewModel
    {
        private readonly IKeyboardServices _keyboardServices;
        private readonly IProductService _productService;
        public ICommand ShowDetailCommand { get; set; }
        public ICommand AddNewCommand { get; set; }


        public ObservableCollection<KeyBoard> KeyBoards { get; set; }

        public KeyboardViewModel(IKeyboardServices keyboardServices, IProductService productService)
        {
            KeyBoards = new ObservableCollection<KeyBoard>();
            _keyboardServices = keyboardServices;
            _productService = productService;
            _ = LoadKeyboards();

            ShowDetailCommand = new RelayCommand<KeyBoard>(ShowDetail);
            AddNewCommand = new RelayCommand(ShowAddNew);
            
        }

        private void ShowAddNew(object obj)
        {
                var addKeyboardView = new KeyboardAddView();
                addKeyboardView.DataContext = new KeyboardAddViewModel(_productService, _keyboardServices);
                addKeyboardView.ShowDialog();
            _ = LoadKeyboards();
        }

        private void ShowDetail(KeyBoard selectedKeyboard)
        {
            if (selectedKeyboard == null)
            {
                MessageBox.Show("Vui lòng chọn một bàn phím để cập nhật hoặc xóa.");// Nếu không có keyboard nào được chọn, khởi tạo keyboard mới
            }

            // Tạo cửa sổ chi tiết và truyền đối tượng Keyboard vào
            var detailWindow = new KeyboardDetail
            {
                DataContext = new KeyboardDetailViewModel(selectedKeyboard, _keyboardServices)
            };

            // Mở cửa sổ và đợi người dùng thao tác
            detailWindow.ShowDialog();

            // Sau khi đóng cửa sổ, bạn có thể tải lại danh sách Keyboard nếu cần
            _ = LoadKeyboards();
        }

        public async Task LoadKeyboards()
        {
            var keyboard = await _keyboardServices.GetListAll();
            KeyBoards.Clear();
            foreach (var key in keyboard)
            {
                KeyBoards.Add(key);
            }
        }
    }
}
