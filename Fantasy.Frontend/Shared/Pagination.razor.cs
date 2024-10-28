using Microsoft.AspNetCore.Components;

namespace Fantasy.Frontend.Shared;

public partial class Pagination
{
    [Parameter] public int PaginaActual { get; set; } = 1;
    [Parameter] public int PaginasTotales { get; set; }
    [Parameter] public int Radio { get; set; } = 2;
    [Parameter] public EventCallback<int> PaginasSeleccionada { get; set; }
    private List<PaginaModel> Enlaces = new List<PaginaModel>();

    private async Task PageSelected(PaginaModel paginaModel)
    {
        if (paginaModel.Pagina == PaginaActual)
        {
            return;
        }
        if (!paginaModel.Habilitada)
        {
            return;
        }
        await PaginasSeleccionada.InvokeAsync(paginaModel.Pagina);
    }

    protected override void OnParametersSet()
    {
        Enlaces = new List<PaginaModel>();

        var enlaceAnteriorHabilitada = PaginaActual != 1;
        var enlaceAnteriorPagina = PaginaActual - 1;
        Enlaces.Add(new PaginaModel
        {
            Texto = "Anterior",
            Pagina = enlaceAnteriorPagina,
            Habilitada = enlaceAnteriorHabilitada
        });

        for (int i = 1; i <= PaginasTotales; i++)
        {
            if (i >= PaginaActual - Radio && i <= PaginaActual + Radio)
            {
                Enlaces.Add(new PaginaModel
                {
                    Texto = i.ToString(),
                    Pagina = i,
                    Activa = PaginaActual == i
                });
            }
        }

        var enlaceSiguienteHabilitado = PaginaActual != PaginasTotales;
        var enlaceSiguientePagina = PaginaActual + 1;
        Enlaces.Add(new PaginaModel
        {
            Texto = "Siguiente",
            Pagina = enlaceSiguientePagina,
            Habilitada = enlaceSiguienteHabilitado
        });
    }

    private class PaginaModel
    {
        public string Texto { get; set; } = null!;

        public int Pagina { get; set; }

        public bool Habilitada { get; set; } = true;

        public bool Activa { get; set; } = false;
    }
}