﻿using HairLumos.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairLumos.Controller
{
    class UsuarioController
    {
        private Models.UsuarioModels _MdlUsu = new Models.UsuarioModels();

        public int gravaUsuario(int cod, int pessoa, string usuario, string senha, int nivel)
        {
            return _MdlUsu.gravaUsuario(cod, pessoa, usuario, senha, nivel);
        }

        public DataTable retornaPessoa(string texto)
        {
            return _MdlUsu.retornaPessoa(texto);
        }

        public DataTable retornaObjUsuario(int cod)
        {
            return _MdlUsu.retornoObjUsuario(cod);
        }

        public DataTable verificaCadastroUser(int cod)
        {
            return _MdlUsu.verificaCadastroUser(cod);
        }

        public bool excluiUsuario(int intCod)
        {
            return _MdlUsu.excluirUsuario(intCod);
        }

        public DataTable realizaLogin(string login, string senha)
        {
            return _MdlUsu.realizaLogin(login, senha);
        }

        public DataTable verificaLogin(string login)
        {
            return _MdlUsu.verificaLogin(login);
        }

        public DataTable existeUsuario()
        {
            return _MdlUsu.existeUsuario();
        }

        public int usuarioLogado(int cod, int codU)
        {
            return _MdlUsu.usuarioLogado(cod, codU);
        }

        public DataTable existeUsuarioLogado()
        {
            return _MdlUsu.existeUsuarioLogado();
        }
    }
}
