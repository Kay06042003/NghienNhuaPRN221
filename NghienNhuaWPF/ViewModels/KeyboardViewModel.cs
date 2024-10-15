using BusinessLogic.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace NghienNhuaWPF.ViewModels
{
    public class KeyboardViewModel : BaseViewModel
    {
        public int KbId { get; set; }
        public string KbLed { get; set; }
        public string KbMode { get; set; }
        public string KbSwitch { get; set; }
        public string KbKeycap { get; set; }
        public string KbPlate { get; set; }
        public string KbCase { get; set; }
        public int ProId { get; set; }
        public virtual Product Pro { get; set; }

        //private KeyBoard _selectedKeyboard;
        //private Product _selectedProduct;

        public KeyboardViewModel() { }

        private readonly IKeyboardServices _keyboardServices;
        public ObservableCollection<KeyBoard> Keyboards { get; set; } = new ObservableCollection<KeyBoard>();
        public async Task LoadKeyboard()
        {
            var keyboards = await _keyboardServices.GetListAll();
            Keyboards.Clear();
            foreach (var keyboard in keyboards)
            {
                Keyboards.Add(keyboard);
            }
        }

        public KeyboardViewModel(IKeyboardServices keyboardServices)
        {
            Keyboards = new ObservableCollection<KeyBoard>();
            _keyboardServices = keyboardServices;
            LoadKeyboard();
        }

    }
}
