using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipoDano
{
    public int quantidadeDano;
    public Collider lugarDano;

    public TipoDano(int danoRecebido, Collider lugarDoDano){
        quantidadeDano = danoRecebido;
        lugarDano = lugarDoDano;
    }
}
