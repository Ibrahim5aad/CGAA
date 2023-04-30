using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Numerics;

namespace CGAA.Extensions
{
    static class PointExtensions
    {

        /// <summary>
        /// Determines whether [is points lies to the right of line] [the specified endpoint1].
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="endpoint1">The first endpoint of the line.</param>
        /// <param name="endpoint2">The second endpoint of the line.</param>
        /// <returns>
        ///   <c>true</c> if [is points lies to the right of line] [the specified endpoint1]; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsPointsLiesToTheRightOfLine(this Point point, Point endpoint1, Point endpoint2)
        {
            Matrix<double> matrix = DenseMatrix.OfArray(new double[,] {
                                                {point.X, point.Y, 1},
                                                {endpoint1.X, endpoint1.Y ,1},
                                                {endpoint2.X, endpoint2.Y ,1}});


            return matrix.Determinant() < 0;
        }
    }
}
