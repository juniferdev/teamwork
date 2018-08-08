using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWork.Internal
{
    public enum NivelProfissional
    {
        Estagiario,
        Trainee,
        Junior,
        Pleno,
        Senior,
        Especialista
    }

    public enum Funcao
    {
        GerenteDeProjetos = 0,
        AnalistaDeRequisitos = 1,
        AnalistaDeNegocios = 2,
        ArquitetoDeSoftware = 3,
        Desenvolvedor = 4,
        TestadorDeSoftware = 5,
        AnalistaDeSuporte = 6,
        UsuarioFinal = 7,
        ParteInteressadaChave = 8
    }

    public enum TipoTarefa
    {
        Tarefa,
        Reuniao,
        Revisao,
        Duvida,
        Teste,
        Bug,
        Indefinido
    }

    public enum Estado
    {
        Aberta,
        Iniciada,
        Feita,
        Encerrada
    }

    public enum Razao
    {
        Criada,
        Aceita,
        Feita,
        Pendente,
        Verificada,
        NaoConcluida,
        Cancelada
    }

    public enum Momento
    {
        Concepcao,
        Requisitos,
        Modelagem,
        Arquitetura,
        Codificacao,
        Testes,
        Manutencao
    }
}