using System.ComponentModel.DataAnnotations;
using Microsoft.CSharp.RuntimeBinder;

namespace Net8LibValid;

/// <summary>
/// Docs
/// </summary>
public class Class1
{
    /// <summary>
    /// Docs
    /// </summary>
    [Range(0, 5)]
    public static int Number { get; set; }

    /// <summary>
    /// Docs
    /// </summary>
    public static void Throw() => throw new RuntimeBinderException();

    private static object X()
    {
        if (Number > 0)
            if (Number > 0)
            {
                Throw();
                throw null!;
            }
            else
            {
                X();
            }

        throw null!;
    }
}