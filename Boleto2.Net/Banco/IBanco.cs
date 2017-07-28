using System.Collections.Generic;

namespace Boleto2Net
{
    public interface IBanco
    {
        Cedente Cedente { get; set; }
        int Codigo { get; }
        string Nome { get; }
        string Digito { get; }
        List<string> IdsRetornoCnab400RegistroDetalhe { get; } // Identifica os registros que cada banco implementa no arquivo retorno, sendo que o primeiro ID da List<> identifica um novo boleto dentro do arquivo retorno.
        bool RemoveAcentosArquivoRemessa { get; }

        /// <summary>
        /// Formata o cedente (Agência, Conta, Código)
        /// </summary>
        void FormataCedente();
        /// <summary>
        /// Formata o campo livre do código de barras
        /// </summary>
        string FormataCodigoBarraCampoLivre(Boleto boleto);
        /// <summary>
        /// Formata o nosso número
        /// </summary>
        void FormataNossoNumero(Boleto boleto);
        /// <summary>
        /// Responsável pela validação de todos os dados referente ao banco, que serão usados no boleto
        /// </summary>
        void ValidaBoleto(Boleto boleto);

        /// <summary>
        /// Gera o header do arquivo de remessa
        /// </summary>
        string GerarHeaderRemessa(ArquivoRemessa arquivo, StatusGeracaoArquivo statusGeracao);
        /// <summary>
        /// Gera o Trailer do arquivo de remessa
        /// </summary>
        string GerarDetalheRemessa(ArquivoRemessa arquivo, Boleto boleto, StatusGeracaoArquivo statusGeracao);
        /// <summary>
        /// Gera o Trailer do arquivo de remessa
        /// </summary>
        string GerarTrailerRemessa(ArquivoRemessa arquivo, StatusGeracaoArquivo statusGeracaoGeracao);

        void LerDetalheRetornoCNAB240SegmentoT(ref Boleto boleto, string registro);
        void LerDetalheRetornoCNAB240SegmentoU(ref Boleto boleto, string registro);
        void LerHeaderRetornoCNAB400(string registro);
        void LerDetalheRetornoCNAB400Segmento1(ref Boleto boleto, string registro);
        void LerDetalheRetornoCNAB400Segmento7(ref Boleto boleto, string registro);
        void LerTrailerRetornoCNAB400(string registro);
    }
}
