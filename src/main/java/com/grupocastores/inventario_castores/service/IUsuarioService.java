package com.grupocastores.inventario_castores.service;

import java.util.List;

import com.grupocastores.inventario_castores.model.Usuario;

public interface IUsuarioService {

    List<Usuario> buscaUsuarios();

    Usuario buscarPorId(Integer idUsuario);
    
    Usuario guardar(Usuario usuario);
    
    void eliminarPorId(Integer id);
}
