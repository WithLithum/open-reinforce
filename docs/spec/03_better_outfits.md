# 03: Improved outfit variations

> [!NOTE]
> The key words "MUST", "MUST NOT", "REQUIRED", "SHALL", "SHALL NOT", "SHOULD", "SHOULD NOT", "RECOMMENDED", "NOT RECOMMENDED", "MAY", and "OPTIONAL" in this document are to be interpreted as described in BCP 14 [RFC2119] [RFC8174] when, and only when, they appear in all capitals, as shown here.

[RFC2119]: https://www.rfc-editor.org/info/rfc2119/
[RFC8174]: https://www.rfc-editor.org/info/rfc8174/

## 1. Schema

Outfit variation consists of an identifier, an optional name, and dictionaries of:

- component slot to drawable and texture
- prop anchor to drawable and texture

## 2. Parsing

When converting LSPDFR outfit variations, the converting routine should first create a map of
script names to LSPDFR outfit variations, called the ***origin map***.

### Base variations

The converting routine MUST respect LSPDFR `base` field.

If `base` field exists, the converting routine first searches for the corresponding base variation,
in the origin map. If such variation does not exist, ignore the `base` field and go on with
the rest of the conversion.

If the base variation exists, the converting routine first applies the values of the base variation
to the resulting dictionaries, then applies the values of the one from the "child" variation.

Chained variation inheritance results in undefined behaviour.
