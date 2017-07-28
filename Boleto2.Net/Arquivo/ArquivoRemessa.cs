using System;
using System.IO;
using System.Text;

namespace Boleto2Net
{
    public class ArquivoRemessa // : AbstractArquivoRemessa, IArquivoRemessa
    {
        public IBanco Banco { get; set; }
        public TipoArquivo TipoArquivo { get; set; }
        public int NumeroArquivoRemessa { get; set; }
        public ArquivoRemessa(IBanco banco, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            Banco = banco;
            TipoArquivo = tipoArquivo;
            NumeroArquivoRemessa = numeroArquivoRemessa;
        }

        public void GerarArquivoRemessa(Boletos boletos, Stream arquivo)
        {
            try
            {
                int tamanhoRegistro;
                if (this.TipoArquivo == TipoArquivo.CNAB240)
                    tamanhoRegistro = 240;
                else
                    tamanhoRegistro = 400;

                StreamWriter arquivoRemessa = new StreamWriter(arquivo, Encoding.GetEncoding("ISO-8859-1"));
                string strline = String.Empty;
                var statusGeracao = new StatusGeracaoArquivo();
                // Header do Arquivo
                strline = Banco.GerarHeaderRemessa(this, statusGeracao);
                if (String.IsNullOrWhiteSpace(strline))
                    throw new Exception("Registro HEADER obrigatório.");
                strline = FormataLinhaArquivoCNAB(strline, tamanhoRegistro);
                arquivoRemessa.WriteLine(strline);

                foreach (Boleto boleto in boletos)
                {
                    // Todos os boletos da coleção devem ser do mesmo banco da geração do arquivo remessa
                    // A solução aqui é forçar essa relação, mas talvez seja melhor subir uma exceção detalhando o erro.
                    boleto.Banco = this.Banco;

                    // Detalhe do arquivo
                    strline = boleto.Banco.GerarDetalheRemessa(this, boleto, statusGeracao);
                    if (String.IsNullOrWhiteSpace(strline))
                        throw new Exception("Registro DETALHE obrigatório.");
                    strline = FormataLinhaArquivoCNAB(strline, tamanhoRegistro);
                    arquivoRemessa.WriteLine(strline);

                    statusGeracao.AjustaTotalizadores(boleto);
                }

                // Trailler do Arquivo
                strline = Banco.GerarTrailerRemessa(this, statusGeracao);
                if (!String.IsNullOrWhiteSpace(strline))
                {
                    strline = FormataLinhaArquivoCNAB(strline, tamanhoRegistro);
                    arquivoRemessa.WriteLine(strline);
                }

                arquivoRemessa.Close();
                arquivoRemessa.Dispose();
                arquivoRemessa = null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar arquivo remessa.", ex);
            }
        }

        private string FormataLinhaArquivoCNAB(string strLinha, int tamanhoRegistro)
        {
            strLinha = strLinha.ToUpper();
            if (Banco.RemoveAcentosArquivoRemessa)
            {
                strLinha = Utils.SubstituiCaracteresEspeciais(strLinha);
            }
            if (tamanhoRegistro != 0)
            {
                string[] strLinhas = strLinha.Split('\n');
                foreach (string s in strLinhas)
                {
                    if (s.Replace("\r", "").Length != tamanhoRegistro)
                        throw new Exception("Registro com tamanho incorreto:" + s.Replace("\r", "").Length.ToString() + " - " + s);
                }
            }
            return strLinha.ToUpper();
        }

    }
}
