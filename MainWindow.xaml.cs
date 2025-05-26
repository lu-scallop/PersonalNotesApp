using PersonalNotesApp.Model;
using PersonalNotesApp.ViewModel;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
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
			rtbConteudo.IsEnabled = false;
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
		private void Negrito_Click(object sender, RoutedEventArgs e)
		{
			TextSelection txtSelecionado = rtbConteudo.Selection;

            if (txtSelecionado != null)
            {
                if (txtSelecionado.GetPropertyValue(TextElement.FontWeightProperty).Equals(FontWeights.Bold))
                {
					txtSelecionado.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
                }
				else
				{
					txtSelecionado.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
				}
			}
		}
		private void Italico_Click(object sender, RoutedEventArgs e)
		{
			TextSelection txtSelecionado = rtbConteudo.Selection;

            if (txtSelecionado != null)
            {
				if (txtSelecionado.GetPropertyValue(TextElement.FontStyleProperty).Equals(FontStyles.Italic))
				{
					txtSelecionado.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
				}
				else
				{
					txtSelecionado.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
				}
			}
        }

		private void Sublinhado_Click(object sender, RoutedEventArgs e)
		{
			TextSelection txtSelecionado = rtbConteudo.Selection;

			if (txtSelecionado != null)
			{
				if (txtSelecionado.GetPropertyValue(Inline.TextDecorationsProperty).Equals(TextDecorations.Underline))
				{
					txtSelecionado.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
				}
				else
				{
					txtSelecionado.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
				}
			}
		}

		private void AlinharEsquerda_Click(object sender, RoutedEventArgs e)
		{
			TextSelection txtSelecionado = rtbConteudo.Selection;
			txtSelecionado.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
		}
		private void AlinharDireita_Click(object sender, RoutedEventArgs e)
		{
			rtbConteudo.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
		}
		private void AlinharCentro_Click(object sender, RoutedEventArgs e)
		{
			rtbConteudo.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
		}
		private void tv_Main_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (e.NewValue is Anotacao anotacaoSelecionado)
			{
				ViewModel.ItemSelecionado = anotacaoSelecionado;
				rtbConteudo.Document = anotacaoSelecionado.Conteudo;
				rtbConteudo.TextChanged -= RtbConteudo_TextChanged;
				rtbConteudo.TextChanged += RtbConteudo_TextChanged;
			}
            else if (e.NewValue is Pasta pastaSelecionado)
            {
				ViewModel.ItemSelecionado = pastaSelecionado;
				rtbConteudo.Document = new FlowDocument();
				rtbConteudo.IsEnabled = false;
				rtbConteudo.TextChanged -= RtbConteudo_TextChanged;
            }
			else
			{
				ViewModel.ItemSelecionado = null;
				rtbConteudo.Document = new FlowDocument();
				rtbConteudo.IsEnabled = false;
				rtbConteudo.TextChanged -= RtbConteudo_TextChanged;
			}
            if (ViewModel.ItemSelecionado is Anotacao)
            {
				rtbConteudo.IsEnabled = true;
            }
        }

		private void RtbConteudo_TextChanged(object sender, TextChangedEventArgs e)
		{
            if (ViewModel.AnotacaoSelecionada != null && rtbConteudo.Document != ViewModel.AnotacaoSelecionada.Conteudo)
            {
				ViewModel.AnotacaoSelecionada.Conteudo = rtbConteudo.Document;
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