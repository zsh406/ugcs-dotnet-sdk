using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Programmerare.CrsTransformations.Coordinate;
using Programmerare.CrsTransformations.CompositeTransformations;
using Programmerare.CrsTransformations;

namespace Services.SdkServices
{
    public class CoordinateTransformation
    {
        public int fromEPSGstring;
        public int toEPSGstring;
        public CoordinateTransformation(int fromEPSG, int toEPSG)
        {
            this.fromEPSGstring = fromEPSG;
            this.toEPSGstring = toEPSG;
        }

        public CrsCoordinate transformPoint(double x, double y)
        {
            CrsCoordinate coordFromNE = CrsCoordinateFactory.NorthingEasting(x, y, this.fromEPSGstring);
            ICrsTransformationAdapter crsTransformationAdapter = CrsTransformationAdapterCompositeFactory.Create().CreateCrsTransformationFirstSuccess();
            // If the NuGet configuration includes all (currently three) adapter implementations, then the 
            // above created 'Composite' implementation will below use all three 'leaf' implementations 
            // and return a coordinate with a median longitude and a median latitude
            CrsTransformationResult coordRes = crsTransformationAdapter.Transform(coordFromNE, this.toEPSGstring);

            if (coordRes.IsSuccess)
            {
                CrsCoordinate outputCoord = coordRes.OutputCoordinate;
                return outputCoord;
            }
            else
            {
                throw (new Exception("Coord transformation failed"));
            }
        }   
            
    }
}
