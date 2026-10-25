/*
 * File: SetConfigurationsExtensions.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 25 October 2026
 *
 * Description:
 * SetConfigurationsExtensions provides extension methods for working with
 * SetsConfigurations instances. It includes a method for determining whether
 * two configurations are equivalent by comparing their empty-set handling,
 * field terminators, row terminators and custom-object settings.
 */
namespace SetSharp.Extensions;

/// <summary>
/// Provides extension methods for working with <see cref="SetsConfigurations"/> instances.
/// </summary>
/// <remarks>
/// These extensions provide additional functionality for comparing set
/// configurations and determining whether their settings are equivalent.
/// </remarks>
public static class SetConfigurationsExtensions
{
    /// <summary>
    /// Determines whether two <see cref="SetsConfigurations"/> instances have
    /// the same configuration values.
    /// </summary>
    /// <param name="configA">
    /// The first configuration to compare.
    /// </param>
    /// <param name="configB">
    /// The second configuration to compare.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if both configurations have matching values for
    /// <see cref="SetsConfigurations.IgnoreEmptySets"/>,
    /// <see cref="SetsConfigurations.FieldTerminator"/>,
    /// <see cref="SetsConfigurations.RowTerminator"/> and
    /// <see cref="SetsConfigurations.IsICustomObject"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Each corresponding property is compared using its
    /// <see cref="object.Equals(object?)"/> method. This method compares the
    /// specified configuration properties rather than checking whether the
    /// two configuration instances are the same object.
    /// </remarks>
    public static bool IsTheSameAs(this SetsConfigurations configA, SetsConfigurations configB)
    {
        if(ReferenceEquals(configA, configB)) return true;

        return configA.IgnoreEmptySets.Equals(configB.IgnoreEmptySets)
            && configA.FieldTerminator.Equals(configB.FieldTerminator)
            && configA.RowTerminator.Equals(configB.RowTerminator)
            && configA.IsICustomObject.Equals(configB.IsICustomObject);
    }
}