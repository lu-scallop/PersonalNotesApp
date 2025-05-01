using PersonalNotesApp.Model;
using PersonalNotesApp.ViewModel;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonalNotesApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainViewModel ViewModel { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			ViewModel = new MainViewModel();
			DataContext = ViewModel;
		}
		private void AdicionaNovaPasta_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.AdicionaNovaPasta();
		}
		private void AdicionaSubPasta_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.AdicionaSubPasta(ViewModel.ItemSelecionado as Pasta);
		}
		private void AdicionaNovaAnotacao_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.AdicionaNovaAnotacao();
		}
		private void AdicionaNovaAnotacaoEmSubPasta_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.AdicionaNovaAnotacaoEmSubPasta(ViewModel.ItemSelecionado as Pasta);
		}
		private void Salvar_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.Salvar(ViewModel.CaminhoRaiz, ViewModel.Pastas);
		}
		private void CarregarPastasEnotas_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.Pastas.Clear();
			ViewModel.MapearPastaEstruturaParaTreeView(ViewModel.CaminhoRaiz, ViewModel.Pastas);
		}
		private void tv_Main_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (e.NewValue is Anotacao anotacaoSelecionado)
			{
				ViewModel.ItemSelecionado = anotacaoSelecionado;
				rtbConetudo.Document = anotacaoSelecionado.Conteudo;
				rtbConetudo.TextChanged -= rtbConetudo_TextChanged;
				rtbConetudo.TextChanged += rtbConetudo_TextChanged;
			}
            if (e.NewValue is Pasta pastaSelecionado)
            {
				ViewModel.ItemSelecionado = pastaSelecionado;
            }
        }
		private void tv_Main_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
            if (tv_Main.SelectedItem is Base item)
            {
				item.EditaNome = true;
            }
        }
		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (((FrameworkElement)sender).DataContext is  Base item)
				item.EditaNome = false;
		}
		private void Sair_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}