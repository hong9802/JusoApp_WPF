using System.Windows;
using System.Windows.Controls;
using System.Collections;

namespace JusoApp_WPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        string url;
        RequestHandler requestHandler = new RequestHandler();
        private ArrayList GetData()
        {
            ArrayList data = new ArrayList();
            for (int i = 0; i < requestHandler.jusoarray.Length; i++)
            {
                data.Add(requestHandler.jusoarray[i] + "\n" + requestHandler.jusolndn[i]);
            }
            return data;
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Jusoinfo_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            url = "http://www.juso.go.kr/support/AddressMainSearch.do?searchKeyword=" + jusoinfo.Text;
            requestHandler.send_RequestAsync(url);
            jusolist.ItemsSource = GetData();
        }

        private void Jusolist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = jusolist.SelectedIndex;
            if (index > -1)
            {
                url = "http://www.juso.go.kr/search/getAddrJuminDetailRenew.do?bdMgtSn=" + requestHandler.jusobdMgtSn[index];
                requestHandler.send_RequestAsync(url);
                MessageBoxResult result = MessageBox.Show("지번 주소 : " + requestHandler.jusolndn[index] + "\n관할 동사무소 : " + requestHandler.KorNM + "\n전화번호 : " + requestHandler.telCn, "도로명 주소 : " + requestHandler.jusoarray[index], MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        url = requestHandler.jusoarray[index];
                        MessageBox.Show("복사했습니다.", url);
                        Clipboard.SetText(url);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private void onKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Return:
                    Search_Click(this, e);
                    break;
            }
        }
    }
    
}
