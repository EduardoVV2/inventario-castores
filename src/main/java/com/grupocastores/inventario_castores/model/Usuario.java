package com.grupocastores.inventario_castores.model;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.OneToOne;
import jakarta.persistence.Table;

@Entity
@Table(name = "usuarios")
public class Usuario {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer idUsuario;

    private String nombre;
    private String correo;
    private String contrasena;

    @OneToOne
    @JoinColumn(name = "idRol")
    private Rol idRol;

    public Integer getIdUsuario(){
        return idUsuario;
    }
    public void setIdUsuario(){
        this.idUsuario = idUsuario;
    }
    public String getNombre(){
        return nombre;
    }
    public void setNombre(){
        this.nombre = nombre;
    }
    public String getCorreo(){
        return correo;
    }
    public void setCorreo(){
        this.correo = correo;
    }
    public String getContrasena(){
        return contrasena;
    }
    public void setContrasena(){
        this.contrasena = contrasena;
    }
    public Rol getIdRol(){
        return idRol;
    }
    public Integer getJustIdRol(){
        return idRol.getIdRol();
    }
    public void setidRol(){
        this.idRol = idRol;
    }

    @Override
    public String toString(){
        return "Usuario [idUsuario=" + idUsuario + ", nombre=" + nombre + ", correo=" + correo + ", contraseña=" + contrasena + ", idRol=" + idRol + "]";
    }

}
