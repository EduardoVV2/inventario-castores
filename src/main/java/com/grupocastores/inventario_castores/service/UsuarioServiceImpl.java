package com.grupocastores.inventario_castores.service;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.grupocastores.inventario_castores.model.Usuario;
import com.grupocastores.inventario_castores.repository.UsuarioRepository;

@Service
public class UsuarioServiceImpl implements IUsuarioService {

    @Autowired
    private UsuarioRepository userRepo;
    
    @Override
    public List<Usuario> buscaUsuarios() {
        return userRepo.findAll();
    }

    @Override
    public Usuario buscarPorId(Integer idUsuario) {
        return userRepo.getById(idUsuario);
    }

    @Override
    public Usuario guardar(Usuario usuario) {
        return userRepo.save(usuario);
    }

    @Override
    public void eliminarPorId(Integer id) {
        userRepo.deleteById(id);
    }

}
