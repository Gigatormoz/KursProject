namespace ProdjectClient.Maui.Pages;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void OnLoginButtonClicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text;
        var password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ErrorLabel.Text = "Заполните все поля";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Временная проверка
        if (email == "test@example.com" && password == "123456")
        {
            // Здесь можно перейти в главное окно
            // Shell.Current.GoToAsync("//main");
            ErrorLabel.Text = "Успешный вход";
            ErrorLabel.TextColor = Colors.Green;
            ErrorLabel.IsVisible = true;
        }
        else
        {
            ErrorLabel.Text = "Неверный email или пароль";
            ErrorLabel.IsVisible = true;
        }
    }

    private void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        // Временно — просто показываем надпись
        ErrorLabel.Text = "Регистрация пока не реализована";
        ErrorLabel.IsVisible = true;
    }
}