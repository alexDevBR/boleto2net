using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boleto2Net
{
    public class StatusGeracaoArquivo
    {
        public int     NumeroRegistroGeral              = 0;
        public int     NumeroRegistroCobrancaSimples    = 0;
        public int     NumeroRegistroCobrancaVinculada  = 0;
        public int     NumeroRegistroCobrancaCaucionada = 0;
        public int     NumeroRegistroCobrancaDescontada = 0;

        public decimal ValorBoletoGeral                 = 0;
        public decimal ValorCobrancaSimples             = 0;
        public decimal ValorCobrancaVinculada           = 0;
        public decimal ValorCobrancaCaucionada          = 0;
        public decimal ValorCobrancaDescontada          = 0;

        internal void AjustaTotalizadores(Boleto boleto)
        {
            ValorBoletoGeral += boleto.ValorTitulo;
            switch (boleto.TipoCarteira)
            {
                case TipoCarteira.CarteiraCobrancaSimples:
                    NumeroRegistroCobrancaSimples++;
                    ValorCobrancaSimples += boleto.ValorTitulo;
                    break;
                case TipoCarteira.CarteiraCobrancaVinculada:
                    NumeroRegistroCobrancaVinculada++;
                    ValorCobrancaVinculada += boleto.ValorTitulo;
                    break;
                case TipoCarteira.CarteiraCobrancaCaucionada:
                    NumeroRegistroCobrancaCaucionada++;
                    ValorCobrancaCaucionada += boleto.ValorTitulo;
                    break;
                case TipoCarteira.CarteiraCobrancaDescontada:
                    NumeroRegistroCobrancaDescontada++;
                    ValorCobrancaDescontada += boleto.ValorTitulo;
                    break;
                default:
                    break;
            }
        }
    }
}
