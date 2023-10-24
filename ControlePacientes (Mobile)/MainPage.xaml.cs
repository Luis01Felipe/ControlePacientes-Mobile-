using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace ControlePacientes__Mobile_
{
    public partial class MainPage : ContentPage
    {
        private IFirebaseClient client;

        public MainPage()
        {
            InitializeComponent();
            client = new FireSharp.FirebaseClient(config);
        }

        IFirebaseConfig config = new FirebaseConfig //DEMO
        {
            AuthSecret = "job3oYnbzoisXeHaAJfOBZQwkQMTfLZ6aSin3Dt2",
            BasePath = "https://democontrolepacientes-default-rtdb.firebaseio.com/"
        };

        async void Update_Clicked(object sender, EventArgs e)
        {
            try
            {
                var firebaseClient = new FireSharp.FirebaseClient(config);
                var existingData = firebaseClient.Get("pacientes/" + entryID.Text);

                if (string.IsNullOrWhiteSpace(entryID.Text))
                {
                    await DisplayAlert("Erro", "É necessário inserir um ID para atualizar!", "OK");
                }
                else if (existingData.Body != "null")
                {
                    var data = firebaseClient.Get("pacientes/" + entryID.Text);

                    if (data.Body != "null")
                    {
                        var paciente = data.ResultAs<Paciente>();

                        if (paciente != null)
                        {
                            // Atualize apenas os campos que foram preenchidos
                            if (!string.IsNullOrEmpty(entryName.Text))
                                paciente.Nome = entryName.Text;
                            if (!string.IsNullOrEmpty(entryDate.Text))
                                paciente.DataQui = entryDate.Text;
                            if (!string.IsNullOrEmpty(entryHospital.Text))
                                paciente.Hospital = entryHospital.Text;
                            if (!string.IsNullOrEmpty(entryArrive.Text))
                                paciente.Chegada = entryArrive.Text;
                            if (!string.IsNullOrEmpty(entryMedicine.Text))
                                paciente.Medicamento = entryMedicine.Text;
                            if (!string.IsNullOrEmpty(entryBefore.Text))
                                paciente.PréQui = entryBefore.Text;
                            if (!string.IsNullOrEmpty(entryDuring.Text))
                                paciente.DuranteQui = entryDuring.Text;
                            if (!string.IsNullOrEmpty(entryAfter.Text))
                                paciente.ApósQui = entryAfter.Text;
                            if (!string.IsNullOrEmpty(entryObs.Text))
                                paciente.Observacoes = entryObs.Text;

                            var response = await firebaseClient.UpdateAsync("pacientes/" + entryID.Text, paciente);

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                await DisplayAlert("Sucesso", "Registro atualizado com sucesso!", "OK");
                            }
                            else
                            {
                                await DisplayAlert("Erro", "Falha ao atualizar o registro. Verifique o ID.", "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Aviso", "Registro não foi carregado corretamente. Verifique a estrutura dos dados no Firebase.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "Nenhum registro encontrado para atualizar.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Aviso", "O registro com o ID fornecido não existe. Não é possível atualizá-lo.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Erro ao atualizar o registro: " + ex.Message, "OK");
            }
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entryID.Text))
                {
                    await DisplayAlert("Erro", "É necessário inserir um ID para excluir!", "OK");
                }
                else if (entryID.Text == "0")
                {
                    await DisplayAlert("Aviso", "Não é possível excluir o registro de ID 0.", "OK");
                }
                else
                {
                    var firebaseClient = new FireSharp.FirebaseClient(config);
                    var existingData = firebaseClient.Get("pacientes/" + entryID.Text);

                    if (existingData.Body != "null")
                    {
                        var response = await firebaseClient.DeleteAsync("pacientes/" + entryID.Text);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            await DisplayAlert("Sucesso", "Registro excluído com sucesso!", "OK");
                            Clean_Clicked(sender, e);
                        }
                        else
                        {
                            await DisplayAlert("Erro", "Falha ao excluir o registro. Verifique o ID.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "O registro com o ID fornecido não existe.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Erro ao excluir o registro: " + ex.Message, "OK");
            }
        }


        async void Add_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var firebaseClient = new FireSharp.FirebaseClient(config);
                var existingData = firebaseClient.Get("pacientes/" + entryID.Text);

                if (string.IsNullOrWhiteSpace(entryID.Text))
                {
                    await DisplayAlert("Erro", "É necessário inserir um ID para adicionar!", "OK");
                }
                else if (existingData.Body != "null")
                {
                    await DisplayAlert("Erro", "ID já existe no banco de dados. Utilize um ID único.", "OK");
                }
                else
                {
                    var data = new
                    {
                        ID = entryID.Text,
                        Nome = entryName.Text,
                        DataQui = entryDate.Text,
                        Hospital = entryHospital.Text,
                        Chegada = entryArrive.Text,
                        Medicamento = entryMedicine.Text,
                        PréQui = entryBefore.Text,
                        DuranteQui = entryDuring.Text,
                        ApósQui = entryAfter.Text,
                        Observações = entryObs.Text
                    };

                    SetResponse response = firebaseClient.Set("pacientes/" + entryID.Text, data);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await DisplayAlert("Sucesso", "Dados Inseridos!", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Erro ao inserir dados no Firebase.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Erro ao inserir o registro: " + ex.Message, "OK");
            }
        }

        async void Select_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entryID.Text))
                {
                    await DisplayAlert("Erro", "É necessário inserir um ID para selecionar!", "OK");
                }
                else
                {
                    var data = client.Get("pacientes/" + entryID.Text);

                    if (data.Body != "null")
                    {
                        var paciente = data.ResultAs<Paciente>();

                        if (paciente != null)
                        {
                            entryName.Text = paciente.Nome;
                            entryDate.Text = paciente.DataQui;
                            entryHospital.Text = paciente.Hospital;
                            entryArrive.Text = paciente.Chegada;
                            entryMedicine.Text = paciente.Medicamento;
                            entryBefore.Text = paciente.PréQui;
                            entryDuring.Text = paciente.DuranteQui;
                            entryAfter.Text = paciente.ApósQui;
                            entryObs.Text = paciente.Observacoes;

                            await DisplayAlert("Sucesso", "Registro encontrado.", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Aviso", "Nenhum registro encontrado para o ID especificado.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "Nenhum registro encontrado para o ID especificado.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Erro ao buscar o registro: " + ex.Message, "OK");
            }
        }

        void Clean_Clicked(object sender, System.EventArgs e)
        {
            entryID.Text = string.Empty;
            entryName.Text = string.Empty;
            entryDate.Text = string.Empty;
            entryHospital.Text = string.Empty;
            entryArrive.Text = string.Empty;
            entryMedicine.Text = string.Empty;
            entryBefore.Text = string.Empty;
            entryDuring.Text = string.Empty;
            entryAfter.Text = string.Empty;
            entryObs.Text = string.Empty;
        }

        async void Help_Clicked(object sender, System.EventArgs e)
        {
            await DisplayAlert("Ajuda", "Digite os dados nas caixas de texto e clique em 'Adicionar' para inserir um novo registro.\n\n" +
                "Para atualizar um registro, digite o ID do registro que deseja atualizar e clique em 'Selecionar'. Os dados do registro serão carregados nas caixas de texto. Altere os dados que deseja atualizar e clique em 'Atualizar'.\n\n" +
                "Para excluir um registro, digite o ID do registro que deseja excluir e clique em 'Excluir'.\n\n" +
                "Para visualizar um registro, digite o ID do registro que deseja visualizar e clique em 'Selecionar' ou clique duas vezes em uma célula da tabela correspondente à linha do registro. Os dados do registro serão carregados nas caixas de texto.\n\n" +
                "Para limpar as caixas de texto, clique em 'Limpar'.\n\n" +
                "Para atualizar a visualização da tabela de registros, clique em 'Atualizar Tabela'.\n\n", "OK");
        }


        // Configura a entrada de ID
        private void onlyNumeric(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue)) return;

            if (!double.TryParse(e.NewTextValue, out double value))
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
        }

        // Configura a entrada de Data
        private void OnEntryDateTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Entry entryDate = (Entry)sender;

                // Limita o tamanho a 10 dígitos
                if (entryDate.Text.Length > 10)
                {
                    entryDate.Text = entryDate.Text.Substring(0, 10);
                }

                if (entryDate.Text.Length == 2 || entryDate.Text.Length == 5)
                {
                    entryDate.Text += "/";
                    entryDate.CursorPosition = entryDate.Text.Length; // Coloca o cursor no final
                }

                // Verifica se o dia (dd) está dentro do limite (1 a 31)
                if (entryDate.Text.Length >= 2 && int.Parse(entryDate.Text.Substring(0, 2)) > 31)
                {
                    entryDate.Text = "31" + entryDate.Text.Substring(2);
                }

                // Verifica se o mês (mm) está dentro do limite (1 a 12)
                if (entryDate.Text.Length >= 5 && int.Parse(entryDate.Text.Substring(3, 2)) > 12)
                {
                    entryDate.Text = entryDate.Text.Substring(0, 3) + "12" + entryDate.Text.Substring(5);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("A cadeia de caracteres de entrada não estava em um formato correto."))
                {
                    DisplayAlert("Erro", "Erro ao alterar a data. Este erro ocorre quando tenta-se apagar um valor antes de alguma barra (/). Por favor, insira uma data válida no formato 'dd/mm/yyyy'.", "OK");
                }
                else if (ex.Message.Contains("Input string was not in a correct format."))
                {
                }
                else
                {
                    DisplayAlert("Erro", "Erro ao alterar a data: " + ex.Message, "OK");
                }
                Entry entryDate = (Entry)sender;
                entryDate.Text = string.Empty;
            }
        }

        // Configura a entrada de Chegada
        private void OnEntryArriveTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Entry entryArrive = (Entry)sender;

                if (entryArrive.Text.Length > 5)
                {
                    entryArrive.Text = entryArrive.Text.Substring(0, 5);
                }

                if (entryArrive.Text.Length == 2 && !entryArrive.Text.Contains(":"))
                {
                    entryArrive.Text += ":";
                    entryArrive.CursorPosition = entryArrive.Text.Length; // Coloca o cursor no final
                }

                // Verifica se as horas (hh) estão dentro do limite (1 a 12)
                if (entryArrive.Text.Length >= 2)
                {
                    int hours = int.Parse(entryArrive.Text.Substring(0, 2));
                    if (hours > 24)
                    {
                        entryArrive.Text = "24" + entryArrive.Text.Substring(2);
                    }
                }

                // Verifica se os minutos (mm) estão dentro do limite (0 a 59)
                if (entryArrive.Text.Length >= 5)
                {
                    int minutes = int.Parse(entryArrive.Text.Substring(3, 2));
                    if (minutes > 59)
                    {
                        entryArrive.Text = entryArrive.Text.Substring(0, 3) + "59";
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("A cadeia de caracteres de entrada não estava em um formato correto."))
                {
                    DisplayAlert("Erro", "Erro ao alterar a hora. Este erro ocorre quando tenta-se apagar um valor antes do dois pontos (:). Por favor, insira um horário válido no formato 'hh:mm'.", "OK");
                }
                else if (ex.Message.Contains("Input string was not in a correct format."))
                {
                }
                else
                {
                    DisplayAlert("Erro", "Erro ao alterar a hora: " + ex.Message, "OK");
                }
                Entry entryArrive = (Entry)sender;
                entryArrive.Text = string.Empty;
            }
        }



    }

    public class Paciente
    {
        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("Nome")]
        public string Nome { get; set; }

        [JsonProperty("DataQui")]
        public string DataQui { get; set; }

        [JsonProperty("Hospital")]
        public string Hospital { get; set; }

        [JsonProperty("Chegada")]
        public string Chegada { get; set; }

        [JsonProperty("Medicamento")]
        public string Medicamento { get; set; }

        [JsonProperty("PréQui")]
        public string PréQui { get; set; }

        [JsonProperty("DuranteQui")]
        public string DuranteQui { get; set; }

        [JsonProperty("ApósQui")]
        public string ApósQui { get; set; }

        [JsonProperty("Observações")]
        public string Observacoes { get; set; }
    }
}
