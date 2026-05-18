namespace Integration.TinyViz.Templating;

public partial class GraphDeserializationSerializationSymmetryTests
{
    public static IEnumerable<TheoryDataRow<string>> Sonnet47
    {
        get
        {
            // --- Scalar edge cases ---

            // false is not covered by the existing bool: true case
            yield return _("bool: false");

            // Negative and large numbers
            yield return _("int: -1");
            yield return _("int: -9999999");
            yield return _("intLarge: 2147483647");

            // Decimal edge cases
            yield return _("decimal: -1.4");
            yield return _("decimal: 0");

            // Explicit null value
            yield return _("nullValue:");

            // --- Strings that look like other YAML types ---
            // These must round-trip as strings, not be reinterpreted as bool/null/int

            yield return _(
                yaml: """
                stringTrue: "true"
                """
            );

            yield return _(
                yaml: """
                stringFalse: "false"
                """
            );

            yield return _(
                yaml: """
                stringNull: "null"
                """
            );

            yield return _(
                yaml: """
                stringInt: "42"
                """
            );

            yield return _(
                yaml: """
                stringDecimal: "3.14"
                """
            );

            yield return _(
                yaml: """
                stringEmpty: ""
                """
            );

            // String containing a colon (would break unquoted YAML)
            yield return _("stringWithColon: 'key: value'");

            // String containing leading/trailing whitespace preserved by quoting
            yield return _(
                yaml: """
                stringWhitespace: "  padded  "
                """
            );

            // --- Multiple sibling scalar keys ---

            yield return _(
                yaml: """
                a: 1
                b: 2
                c: 3
                """
            );

            yield return _(
                yaml: """
                enabled: true
                count: 42
                label: Hello
                ratio: 0.5
                """
            );

            // --- Nested maps ---

            yield return _(
                yaml: """
                nested:
                  key: value
                """
            );

            yield return _(
                yaml: """
                outer:
                  inner:
                    deepKey: deepValue
                """
            );

            // Three levels deep with siblings at each level
            yield return _(
                yaml: """
                level1:
                  level1Key: level1Value
                  level2:
                    level2Key: level2Value
                    level3:
                      level3Key: level3Value
                """
            );

            // Sibling top-level maps
            yield return _(
                yaml: """
                first:
                  a: 1
                second:
                  b: 2
                """
            );

            // --- Null inside a map ---

            yield return _(
                yaml: """
                parent:
                  present: value
                  missing:
                """
            );

            // --- Lists of strings that resemble other types ---

            yield return _(
                yaml: """
                listStrings:
                  - "true"
                  - "null"
                  - "0"
                """
            );

            // --- Deeply nested list inside a map ---

            yield return _(
                yaml: """
                config:
                  tags:
                    - alpha
                    - beta
                    - gamma
                """
            );

            // Map nested inside a list item that contains its own list
            yield return _(
                yaml: """
                listNested:
                  - name: group1
                    items:
                      - a
                      - b
                  - name: group2
                    items:
                      - c
                      - d
                """
            );

            // --- Mixed scalar types inside a list ---

            yield return _(
                yaml: """
                listMixed:
                  - 1
                  - true
                  - hello
                """
            );

            // --- Empty map and empty list as siblings ---

            yield return _(
                yaml: """
                emptyMap: {}
                emptyList: []
                """
            );

            // Empty nested map alongside a populated one
            yield return _(
                yaml: """
                populated:
                  key: value
                empty: {}
                """
            );
        }
    }
}
