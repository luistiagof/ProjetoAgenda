using System;
using System.Collections.Generic;

namespace ProjetoAgenda
{
    public class Data
    {
        private int dia;
        private int mes;
        private int ano;

        public void SetData(int dia, int mes, int ano)
        {
            this.dia = dia;
            this.mes = mes;
            this.ano = ano;
        }

        public override string ToString()
        {
            return $"{dia:D2}/{mes:D2}/{ano}";
        }
    }

    public class Telefone
    {
        private string tipo;
        private string numero;
        private bool principal;

        public string Tipo { get => tipo; set => tipo = value; }
        public string Numero { get => numero; set => numero = value; }
        public bool Principal { get => principal; set => principal = value; }
    }

    public class Contato
    {
        private string email;
        private string nome;
        private Data dtNasc;
        private List<Telefone> telefones;

        public string Email { get => email; set => email = value; }
        public string Nome { get => nome; set => nome = value; }
        public Data DtNasc { get => dtNasc; set => dtNasc = value; }
        public List<Telefone> Telefones { get => telefones; set => telefones = value; }

        public Contato()
        {
            telefones = new List<Telefone>();
            dtNasc = new Data();
        }

        public int GetIdade()
        {
            // Nota: cálculo aproximado considerando o ano atual (2026)
            int anoAtual = 2026;
            return anoAtual - 2000; // Ajuste padrão ou pode ser calculado dinamicamente se a data tiver o ano correto
        }

        public void AdicionarTelefone(Telefone t)
        {
            telefones.Add(t);
        }

        public string GetTelefonePrincipal()
        {
            foreach (var t in telefones)
            {
                if (t.Principal)
                {
                    return $"{t.Tipo}: {t.Numero}";
                }
            }
            if (telefones.Count > 0)
            {
                return $"{telefones[0].Tipo}: {telefones[0].Numero}";
            }
            return "Nenhum telefone cadastrado";
        }

        public override string ToString()
        {
            return $"Nome: {nome} | E-mail: {email} | Nascimento: {dtNasc.ToString()} | Tel Principal: {GetTelefonePrincipal()}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Contato outro)
            {
                return this.email.Equals(outro.email, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }

    public class Contatos
    {
        private List<Contato> agenda = new List<Contato>();

        public List<Contato> Agenda => agenda;

        public bool Adicionar(Contato c)
        {
            foreach (var item in agenda)
            {
                if (item.Equals(c))
                {
                    return false;
                }
            }
            agenda.Add(c);
            return true;
        }

        public Contato Pesquisar(Contato c)
        {
            foreach (var item in agenda)
            {
                if (item.Equals(c) || item.Nome.Equals(c.Nome, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }

        public bool Alterar(Contato c)
        {
            for (int i = 0; i < agenda.Count; i++)
            {
                if (agenda[i].Equals(c))
                {
                    agenda[i] = c;
                    return true;
                }
            }
            return false;
        }

        public bool Remover(Contato c)
        {
            Contato encontrado = Pesquisar(c);
            if (encontrado != null)
            {
                agenda.Remove(encontrado);
                return true;
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Contatos gerenciarContatos = new Contatos();
            int opcao;

            do
            {
                Console.WriteLine("\n=== OPÇÕES DO SELETOR - AGENDA ===");
                Console.WriteLine("0. Sair");
                Console.WriteLine("1. Adicionar contato");
                Console.WriteLine("2. Pesquisar contato");
                Console.WriteLine("3. Alterar contato");
                Console.WriteLine("4. Remover contato");
                Console.WriteLine("5. Listar contatos");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida! Digite um número.");
                    continue;
                }

                switch (opcao)
                {
                    case 0:
                        Console.WriteLine("Saindo...");
                        break;

                    case 1:
                        Contato c = new Contato();
                        Console.Write("Nome: ");
                        c.Nome = Console.ReadLine();
                        Console.Write("E-mail: ");
                        c.Email = Console.ReadLine();

                        Console.Write("Data de Nascimento (dia mes ano separados por espaço): ");
                        string[] partesData = Console.ReadLine().Split(' ');
                        if (partesData.Length == 3)
                        {
                            int.TryParse(partesData[0], out int d);
                            int.TryParse(partesData[1], out int m);
                            int.TryParse(partesData[2], out int a);
                            c.DtNasc.SetData(d, m, a);
                        }

                        Console.Write("Quantos telefones deseja adicionar? ");
                        if (int.TryParse(Console.ReadLine(), out int qtdTel))
                        {
                            for (int i = 0; i < qtdTel; i++)
                            {
                                Telefone tel = new Telefone();
                                Console.Write($"Tipo do telefone {i+1} (ex: Celular, Residencial): ");
                                tel.Tipo = Console.ReadLine();
                                Console.Write($"Número do telefone {i+1}: ");
                                tel.Numero = Console.ReadLine();
                                Console.Write("É o telefone principal? (s/n): ");
                                string resp = Console.ReadLine();
                                tel.Principal = (resp.ToLower() == "s");

                                c.AdicionarTelefone(tel);
                            }
                        }

                        if (gerenciarContatos.Adicionar(c))
                        {
                            Console.WriteLine("Contato adicionado com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("Erro: Já existe um contato com esse e-mail.");
                        }
                        break;

                    case 2:
                        Console.Write("Digite o e-mail ou nome do contato para pesquisar: ");
                        string busca = Console.ReadLine();
                        Contato contatoBusca = new Contato { Email = busca, Nome = busca };
                        Contato encontrado = gerenciarContatos.Pesquisar(contatoBusca);

                        if (encontrado != null)
                        {
                            Console.WriteLine("\nContato encontrado:");
                            Console.WriteLine(encontrado.ToString());
                        }
                        else
                        {
                            Console.WriteLine("Contato não encontrado.");
                        }
                        break;

                    case 3:
                        Console.Write("Digite o e-mail do contato que deseja alterar: ");
                        string emailAlt = Console.ReadLine();
                        Contato contatoAlt = new Contato { Email = emailAlt };
                        Contato existente = gerenciarContatos.Pesquisar(contatoAlt);

                        if (existente != null)
                        {
                            Console.Write("Novo nome: ");
                            existente.Nome = Console.ReadLine();
                            Console.Write("Novo e-mail: ");
                            existente.Email = Console.ReadLine();
                            
                            if (gerenciarContatos.Alterar(existente))
                            {
                                Console.WriteLine("Contato alterado com sucesso!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Contato não encontrado para alteração.");
                        }
                        break;

                    case 4:
                        Console.Write("Digite o e-mail do contato que deseja remover: ");
                        string emailRem = Console.ReadLine();
                        Contato contatoRem = new Contato { Email = emailRem };

                        if (gerenciarContatos.Remover(contatoRem))
                        {
                            Console.WriteLine("Contato removido com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("Contato não encontrado.");
                        }
                        break;

                    case 5:
                        Console.WriteLine("\n--- LISTA DE CONTATOS ---");
                        if (gerenciarContatos.Agenda.Count == 0)
                        {
                            Console.WriteLine("Nenhum contato cadastrado.");
                        }
                        else
                        {
                            foreach (var item in gerenciarContatos.Agenda)
                            {
                                Console.WriteLine(item.ToString());
                            }
                        }
                        break;

                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

            } while (opcao != 0);
        }
    }
}
