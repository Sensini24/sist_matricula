using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Sistema_Matricula.Models;
using Stripe;

namespace Sistema_Matricula.Service
{
    public class PagoService
    {
        private readonly DbMatNotaHorarioContext db;

        public PagoService(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public async Task<string> ProcesarPagoMatricula(int idMatricula, string tokenTarjeta)
        {
            var matricula = await db.Matriculas.FindAsync(idMatricula);
            if(matricula == null)
                throw new Exception("Matrícula no encontrada");

            var estudiante = await db.Estudiantes.Where(x=>x.IdEstudiante == matricula.IdEstudiante).FirstAsync();

            var monto = db.Montos.Where(x=>x.IdMonto == matricula.IdMonto).First();

            var options = new ChargeCreateOptions
            {
                Amount = (long)(monto.Monto1 * 100), // Stripe usa centavos
                Currency = "pen",
                Source = tokenTarjeta,
                Description = $"Pago de matrícula para {estudiante.Nombre} {estudiante.Apellido}"
            };

            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);

            if (charge.Status == "succeeded")
            {
                var pago = new Pago
                {
                    MontoPago = monto.Monto1,
                    FechPago = DateTime.Now,
                    TipoPago = charge.PaymentMethodDetails.Type,
                    Estado = "Pagado",
                    IdEstudiante = matricula.IdEstudiante,
                    IdConcepto = 1
                };

                db.Pagos.Add(pago);
                matricula.Estado = "Activo";
                estudiante.Estado = "Activo";

                var factura = new Factura
                {
                    IdPago = pago.IdPago,
                    FechEmision = DateTime.Now,
                    MontoTotal = pago.MontoPago
                };

                db.Pagos.Add(pago);
                db.Facturas.Add(factura);
                await db.SaveChangesAsync();

                return charge.Id;
            }
            else
            {
                throw new Exception("El pago no pudo ser procesado");
            }
        }

    }
}
