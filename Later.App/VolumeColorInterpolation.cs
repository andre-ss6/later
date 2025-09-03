namespace Later.App;

internal static class VolumeColorInterpolation
{
    /// <summary>
    /// Map value [0..100] -> Color biased so:
    /// - value <= 10 => pure green
    /// - value >= 30 => pure red
    /// - 10..30 => smooth biased transition (green -> yellow -> red)
    /// </summary>
    public static Color ValueToBiasedHeatColor(double value)
    {
        value = Math.Clamp(value, 0.0, 100.0);

        // Hard lower/upper regions
        if (value <= 10.0) return ColorFromHSV(120.0, 0.8, 0.6); // green
        if (value >= 30.0) return ColorFromHSV(0.0, 0.8, 0.6);   // red

        // Normalized position between 10 and 30
        double t = (value - 10.0) / 20.0; // 0 -> at 10, 1 -> at 30

        // Bias: exponent < 1 makes the mapping move faster toward red (heavy bias).
        // Adjust bias <1 to be more aggressive, >1 to be more gradual.
        const double biasExponent = 0.5;
        double tBiased = Math.Pow(t, biasExponent);

        // Interpolate hue from 120 (green) to 0 (red)
        double hue = 120.0 * (1.0 - tBiased);

        return ColorFromHSV(hue, 0.8, 0.6);
    }

    private static Color ColorFromHSV(double hue, double saturation, double value)
    {
        hue = (hue % 360 + 360) % 360; // normalize
        double c = value * saturation;
        double x = c * (1 - Math.Abs((hue / 60.0) % 2 - 1));
        double m = value - c;
        double r1;
        double g1;
        double b1;

        if (hue < 60) { r1 = c; g1 = x; b1 = 0; }
        else if (hue < 120) { r1 = x; g1 = c; b1 = 0; }
        else if (hue < 180) { r1 = 0; g1 = c; b1 = x; }
        else if (hue < 240) { r1 = 0; g1 = x; b1 = c; }
        else if (hue < 300) { r1 = x; g1 = 0; b1 = c; }
        else { r1 = c; g1 = 0; b1 = x; }

        byte r = (byte)Math.Round((r1 + m) * 255.0);
        byte g = (byte)Math.Round((g1 + m) * 255.0);
        byte b = (byte)Math.Round((b1 + m) * 255.0);

        return Color.FromArgb(r, g, b);
    }
}
    