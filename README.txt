MIPSCompiler

    Attempt to create a basic scripting language (MIPScript?) that compiles to MIPS Assembly Language

Roadmap

    1) Basic expression parsing:                                                 DONE      (7th March, 2017)
    2) Advanced expression parsing:                                              Started   (8th March, 2017)
          - Parentheses                                                          Started   (8th March, 2017)
          - Register use tracking
          - Add immediate
          - Variables/Assignment
          - Preferential siding         For prettier Assembly
          - Duplication optimization    (a+b)+(a+b)/c calculates a+b twice
          - Shifting operations
          - Replacement optimization    a+b + a+b should turn to (a+b)<<2
          - Arrays
          - ?
    3) If statement                                                              Projected to start 20 March, 2017
          - ?
    4) Loops
          - ?
    5) Functions
          - ?
    6) ?

This project is licensed under the MIT License - see the LICENSE.md file for details