using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KmlEditorLibrary
{
    public class FerromapasKmlHelper
    {
        public static void AddFerromapasSchemaIfNotExists(KmlFile kmlFile)
        {
            Document document = (kmlFile.Root as Kml).Feature as Document;
            var schemas = ((SharpKml.Dom.Document)((SharpKml.Dom.Kml)kmlFile.Root).Feature).Schemas;

            Schema ferromapasEstacionSchema = KmlFileHelper.FindSchemaByName(kmlFile, "Ferromapas_Estacion");
            if (ferromapasEstacionSchema == null)
            {
                ferromapasEstacionSchema = new Schema();
                ferromapasEstacionSchema.Name = "Ferromapas_Estacion";
                ferromapasEstacionSchema.AddField(KmlFileHelper.setSimpleField("Nombre", "String"));
                ferromapasEstacionSchema.AddField(KmlFileHelper.setSimpleField("Ferrocarril", "String"));
                ferromapasEstacionSchema.AddField(KmlFileHelper.setSimpleField("Ramal", "String"));
                ferromapasEstacionSchema.AddField(KmlFileHelper.setSimpleField("Progresiva", "String"));
                document.AddSchema(ferromapasEstacionSchema);
            }


            Schema ferromapasViaSchema = KmlFileHelper.FindSchemaByName(kmlFile, "Ferromapas_Via");
            if (ferromapasViaSchema == null)
            {
                ferromapasViaSchema = new Schema();
                ferromapasViaSchema.Name = "Ferromapas_Via";
                ferromapasViaSchema.AddField(KmlFileHelper.setSimpleField("Nombre", "String"));
                ferromapasViaSchema.AddField(KmlFileHelper.setSimpleField("InicioProgresiva", "String"));
                document.AddSchema(ferromapasViaSchema);
            }

            Schema ferromapasCarpetaSchema = KmlFileHelper.FindSchemaByName(kmlFile, "Ferromapas_Carpeta");
            if (ferromapasCarpetaSchema == null)
            {
                ferromapasCarpetaSchema = new Schema();
                ferromapasCarpetaSchema.Name = "Ferromapas_Carpeta";
                ferromapasCarpetaSchema.AddField(KmlFileHelper.setSimpleField("Nombre", "String"));
                ferromapasCarpetaSchema.AddField(KmlFileHelper.setSimpleField("Autores", "String"));
                ferromapasCarpetaSchema.AddField(KmlFileHelper.setSimpleField("Fuentes", "String"));
                document.AddSchema(ferromapasCarpetaSchema);
            }
        }

    }
}
