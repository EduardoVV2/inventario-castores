package com.grupocastores.inventario_castores.model;

import java.time.LocalDateTime;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.OneToOne;
import jakarta.persistence.Table;

@Entity
@Table(name = "movimientos")
public class Movimiento {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "idMovimiento")
    private Integer idMovimiento;

    @OneToOne
    @JoinColumn(name = "idProducto")
    private Producto producto;

    @OneToOne
    @JoinColumn(name = "idUsuario")
    private Usuario usuario;

    private String tipo;

    private Integer cantidad;

    private LocalDateTime fecha_hora;

    public Integer getId(){
        return idMovimiento;
    }
    public void setId(Integer id){
        this.idMovimiento = id;
    }

    public Producto getProducto(){
        return producto;
    }
    public void setProducto(Producto p){
        this.producto = p;
    }

    public Usuario getUsuario(){
        return usuario;
    }
    public void setUsuario(Usuario usr){
        this.usuario = usr;
    }

    public String getTipo(){
        return tipo;
    }
    public void setTipo(String tipo){
        this.tipo = tipo;
    }

    public Integer getCantidad(){
        return cantidad;
    }
    public void setCantidad(Integer cant){
        this.cantidad = cant;
    }

    public LocalDateTime getFecha_Time(){
        return fecha_hora;
    }
    public void setFecha_Time(LocalDateTime fechaHora){
        this.fecha_hora = fechaHora;
    }

}
