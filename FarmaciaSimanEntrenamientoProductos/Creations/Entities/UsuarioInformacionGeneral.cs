using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FarmaciaSimanEntrenamientoProductos.Creation.Entities
{
    public class UsuarioInformacionGeneral
    {
        [Key]
        public int ID { get; set; }
        public string Usuario { get; set; }
        public int Perfil_ID { get; set; }

        public int AplicacionId { get; set; }
        public byte[] Clave { get; set; }
        public bool Activo { get; set; }
    }
}
