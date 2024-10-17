namespace Flapbird;

public partial class MainPage : ContentPage
{
	const int gravidade = 5;
	int Score = 0;
	const int aberturaMinima = 200;
	const int tempoEntreFrames = 20;
	bool estaMorto = false;
	double larguraJanela = 0;
	double AlturaJanela = 0;
	int velocidade = 10;
	const int ForcaPulo = 30;
	const int MaximoTempoPulando = 3; //frames
	bool EstaPulando = false;
	int TempoPulando = 0;

	public MainPage()
	{
		InitializeComponent();
	}
	void AplicaGravidade()
	{
		Passaro.TranslationY += gravidade;
	}
	public async void Desenha()
	{
		while (!estaMorto)
		{
			if (EstaPulando)
				AplicaPulo();
			else
				AplicaGravidade();
			GerenciaCanos();
			if (VerificaColisao())
			{
				estaMorto = true;
				FrameGameOver.IsVisible = true;
				break;
			}
			await Task.Delay(tempoEntreFrames);
		}
	}
	void AplicaPulo()
	{
		Passaro.TranslationY -= ForcaPulo;
		TempoPulando++;
		if (TempoPulando >= MaximoTempoPulando)
		{
			EstaPulando = false;
			TempoPulando = 0;
		}
	}
	void OnGridClicked(object s, TappedEventArgs a)
	{
		EstaPulando = true;
	}

	void Oi(object s, TappedEventArgs e)
	{
		FrameGameOver.IsVisible = false;
		estaMorto = false;
		Inicializar();
		Desenha();
	}

	void Inicializar()
	{
		Passaro.TranslationY = 0;
	}


	protected override void OnSizeAllocated(double w, double h)
	{
		base.OnSizeAllocated(w, h);
		larguraJanela = w;
		AlturaJanela = h;
	}

	void GerenciaCanos()
	{
		imgcanocima.TranslationX -= velocidade;
		imgcanobaixo.TranslationX -= velocidade;
		if (imgcanobaixo.TranslationX < -larguraJanela)
		{
			imgcanobaixo.TranslationX = 0;
			imgcanocima.TranslationX = 0;

			var alturaMax = -100;
			var alturaMin = -imgcanobaixo.HeightRequest;
			imgcanocima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			imgcanobaixo.TranslationY = imgcanocima.TranslationY + aberturaMinima + imgcanobaixo.HeightRequest;

			Score ++;
			LabelScore.Text = "Canos: " + Score.ToString("D3");
		}


	}
	bool VerificaColisao()
	{
		if (!estaMorto)
		{
			if (VerificaColisaoTeto() ||
			VerificaColisaoChao())
			{
				return true;
			}

		}
		return false;
	}
	bool VerificaColisaoTeto()
	{
		var minY = -AlturaJanela / 2;
		if (Passaro.TranslationY <= minY)
			return true;
		else
			return false;
	}
	bool VerificaColisaoChao()
	{
		var maxY = AlturaJanela / 2 - chao.HeightRequest;
		if (Passaro.TranslationY >= maxY)
			return true;
		else
			return false;

	}

}

