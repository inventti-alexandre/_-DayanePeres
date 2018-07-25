﻿using HairLumos.Banco;
using HairLumos.Entidades;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairLumos.DAO
{
    class usuarioDAO
    {
        private string _sql;

        public usuarioDAO()
        {
            this._sql = string.Empty;
        }

        public DataTable RetornaPessoa(string texto)
        {
            DataTable dt = new DataTable();

            _sql = "SELECT codusuario, codpessoa, usu_usuario, usu_senha, usu_nivel" +
                        " FROM tbusuario";

            int intCodigo = 0;
            
            int.TryParse(texto, out intCodigo);

            if (intCodigo > 0)
                _sql += $"OR codusuario = @codusuario ";

           // _sql += $"OR UPPER (usu_usuario) LIKE @usu_usuario";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(_sql, Conexao.getIntancia().openConn());

                cmd.CommandText = _sql;
                cmd.Parameters.AddWithValue("@codusuario");
                cmd.Parameters.AddWithValue("@codpessoa");
                cmd.Parameters.AddWithValue("@usu_usuario");
                cmd.Parameters.AddWithValue("@usu_senha");
                cmd.Parameters.AddWithValue("@usu_nivel");


                NpgsqlDataReader dr = cmd.ExecuteReader(); //ExecuteReader para select retorna um DataReader
                dt.Load(dr);//Carrego o DataReader no meu DataTable
                dr.Close();//Fecho o DataReader
            }
            catch (Exception e)
            {

                throw new SystemException(e + "Erro ao retornar Usuário");
            }
            return dt;
        }

        public DataTable RetornaPessoaCod(int cod)
        {
            DataTable dt = new DataTable();

            _sql = "SELECT codusuario, codpessoa, usu_usuario, usu_senha, usu_nivel" +
                        " FROM tbusuario WHERE codusuario = "+cod;


            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(_sql, Conexao.getIntancia().openConn());

                cmd.CommandText = _sql;
                cmd.Parameters.AddWithValue("@codusuario");
                cmd.Parameters.AddWithValue("@codpessoa");
                cmd.Parameters.AddWithValue("@usu_usuario");
                cmd.Parameters.AddWithValue("@usu_senha");
                cmd.Parameters.AddWithValue("@usu_nivel");


                NpgsqlDataReader dr = cmd.ExecuteReader(); //ExecuteReader para select retorna um DataReader
                dt.Load(dr);//Carrego o DataReader no meu DataTable
                dr.Close();//Fecho o DataReader
            }
            catch (Exception e)
            {

                throw new SystemException(e + "Erro ao retornar Usuário");
            }
            return dt;
        }

        public int VerificaUsuarioCadastro(int cod)
        {
            

            _sql = "SELECT codusuario, codpessoa, usu_usuario, usu_senha, usu_nivel" +
                        " FROM tbusuario WHERE codusuario = " + cod;


            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(_sql, Conexao.getIntancia().openConn());

                cmd.CommandText = _sql;
                
                NpgsqlDataReader dr = cmd.ExecuteReader(); //ExecuteReader para select retorna um DataReader
                return 1;
            }
            catch (Exception e)
            {

                throw new SystemException(e + "Erro ao retornar Usuário");
                
            }
            return 0;
        }

        public bool ExcluirUsuario(int intCod)
        {

            _sql = "DELETE FROM tbusuario" +
                    " WHERE codusuario = "+ intCod;

            int _controle = 0;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(_sql, Conexao.getIntancia().openConn());

                cmd.Parameters.AddWithValue("@cod", intCod);
                _controle = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            return (_controle > 0);
        }

        public int GravaUsuario(Usuario objPessoa)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(_sql, Conexao.getIntancia().openConn());

            //int _controle = 0;
            try
            {
                if (objPessoa.UsuarioCodigo == 0)
                {

                    _sql = "INSERT INTO tbusuario( codpessoa, usu_usuario, usu_senha, usu_nivel)" +
                      " VALUES(@codPessoa, @usuario, @senha, @nivel)";
                    
                }
                else
                {
                    _sql = "UPDATE tbusuario " +
                        "SET usu_usuario = @user, usu_senha = @senha, usu_nivel = @nivel" +
                        " WHERE codusuario = @codUser";
                }

                cmd.CommandText = _sql;
                cmd.Parameters.AddWithValue("@codUser", objPessoa.UsuarioCodigo);
                cmd.Parameters.AddWithValue("@codPessoa", objPessoa.PessoaCod);
                cmd.Parameters.AddWithValue("@usuario", objPessoa.Login);
                cmd.Parameters.AddWithValue("@senha", objPessoa.Senha);
                cmd.Parameters.AddWithValue("@nivel", objPessoa.Nivel);

                cmd.ExecuteNonQuery();

                return 1;

            }
            catch (Exception E)
            {
                return 0;
            }
            
        }
    }
}